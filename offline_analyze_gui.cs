using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;//nuget 包管理器中安装  //https://www.newtonsoft.com/json
using Newtonsoft.Json.Linq;
using SeeSharpTools.JY.DSP.SoundVibration;
using SeeSharpTools.JY.ArrayUtility;
using SeeSharpTools.JXI.SignalProcessing.Measurement;
using SeeSharpTools.JXI.SignalProcessing.GeneralSpectrum;
using SeeSharpTools.JXI.SignalProcessing.Window;



namespace wfd_demo
{
    public partial class MainFrom : Form
    {
        private string filename;
        private string last_flle_path;


        public MainFrom()
        {
            InitializeComponent();

        }


        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        private const int WM_VSCROLL = 277;
        private const int SB_PAGEBOTTOM = 7;

        public static void ScrollToBottom(RichTextBox MyRichTextBox)
        {
            SendMessage(MyRichTextBox.Handle, WM_VSCROLL, (IntPtr)SB_PAGEBOTTOM, IntPtr.Zero);
        }


        private void showLog(string log)
        {

            if (richTextBoxLog.IsHandleCreated == false) return;

            this.Invoke((EventHandler)(delegate
            {
                richTextBoxLog.SelectionColor = Color.Black;
                richTextBoxLog.AppendText(log + "\r\n");
                ScrollToBottom(richTextBoxLog); // richTextBoxLog.ScrollToCaret();

            }));
        }





        public static Mutex mutex = new Mutex();

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (last_flle_path == null || System.IO.Directory.Exists(last_flle_path) == false)
            {

                openFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);//注意这里写路径时要用c://而不是c:/                
            }
            else
            {

                openFileDialog.InitialDirectory = last_flle_path;//注意这里写路径时要用c://而不是c:/
            }


            //openFileDialog.Filter = "文本文件|*.*|C#文件|*.cs|所有文件|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog.FileName;
                textBox_fileName.Text = filename;
                showLog("File name:" + filename + "\n");
                showLog("File size:" + new System.IO.FileInfo(filename).Length + "\n");

                last_flle_path = new System.IO.FileInfo(filename).DirectoryName;
                // showLog("openFileDialog:" + last_flle_path);
            }

        }

        struct PACK
        {
            public UInt32 len;
            public UInt32 d_ty;
            public UInt32 t0_id;
            public UInt32 tg_id;
            public UInt64 tof;
            public double rsv;
            public UInt32 ch_id;
            public UInt16[] data;
        }

        List<PACK> packs = new List<PACK>();




        private void unpackdata()
        {
            bool nonstop;

            byte[] readdata;

            FileStream F = new FileStream(filename, FileMode.Open, FileAccess.Read);

            BinaryReader G = new BinaryReader(F);

            int n = 0, k, p;

            UInt32 l;

            while (true)
            {
                nonstop = true;
                while (nonstop)
                {
                    try
                    {
                        l = G.ReadUInt32();          //l仅仅是用来检查数据帧头是否出错的，没有绘图的实际意义
                    }
                    catch
                    {
                        if (n == 0)
                        {
                            showLog("no pack is found in" + filename);
                        }
                        F.Close();
                        return;
                    }

                    if (l != 0x000090eb)
                    {
                        showLog("pack head is not found, skip four bytes");
                    }
                    else nonstop = false;
                }

                readdata = G.ReadBytes(4);

                Array.Reverse(readdata);

                UInt32 tmplen;

                tmplen = BitConverter.ToUInt32(readdata, 0);

                n++;

                PACK pACK = new PACK();



                pACK.len = tmplen;

                byte[] temp;

                temp = G.ReadBytes(40);

                Array.Reverse(temp);





                pACK.ch_id = BitConverter.ToUInt32(temp, 0);

                //pACK.rsv = BitConverter.ToUInt32(temp, 4);

                pACK.tof = BitConverter.ToUInt64(temp, 20);

                pACK.tg_id = BitConverter.ToUInt32(temp, 28);

                pACK.t0_id = BitConverter.ToUInt32(temp, 32);

                pACK.d_ty = BitConverter.ToUInt32(temp, 36);

                k = (int)tmplen - 48;                          //表示总共需要读取的字节数

                pACK.data = new UInt16[k/2];

                for (p = 0; p < k / 2; p++)
                {
                    readdata = G.ReadBytes(2);

                    Array.Reverse(readdata);

                    pACK.data[p] = BitConverter.ToUInt16(readdata, 0);
                }


                packs.Add(pACK);

            }
        }






        private void button1_Click(object sender, EventArgs e)
        {
            unpackdata();

            int len = packs[0].data.Length;

            int i,j;

            double[] packed = new double[len];

            double[] ave = new double[30];                //ave是平均值

            double[][] unpacked = new double[30][];

            double[] ENOB = new double[30],SINAD=new double[30],SNR=new double[30],THD=new double[30];

            double finalENOB=0, finalSINAD=0, finalSNR=0, finalTHD=0;

            for (j = 0; j< 30; j++)
            {
                double[] test = new double[len];
                for (i=0;i<len;i++)
                {
                    test[i] = (double)packs[j].data[i];
                    ave[j] = ave[j] + test[i];
                    //unpacked[i] = (unpacked[i] - 2048.0) / 2048.0;
                }
                ave[j] = ave[j] / len;
                unpacked[j] = test;
                for(i=0;i<len;i++)
                {
                    packed[i] = test[i] - ave[j];
                }
                //load waveform
                double dt = 1 / (double)51200;
                //THD analysis
                double fundamentalFreq;
                double[] componentsLevel = new double[0];
                double thd;
                double sinad;
                //double[] signal = {0};
                ToneAnalysisResult toneAnalysisResult = HarmonicAnalyzer.ToneAnalysis(packed, 1, 10, true);
                //ArrayCalculation.AddOffset(ref packed, 2);

                /*
                public static void PowerSpectrum(double[] x, double samplingRate, ref double[] spectrum, out double df, SpectrumOutputUnit unit = SpectrumOutputUnit.V2, WindowType windowType = WindowType.Hanning, double windowPara = double.NaN, bool PSD = false)
                {
                    int num = spectrum.Length;
                    SpectralInfo spectralInfo = default(SpectralInfo);
                    AdvanceRealFFT(x, num, windowType, spectrum, ref spectralInfo);
                    double alpha = 1.0 / (double)spectralInfo.FFTSize;
                    CBLASNative.cblas_dscal(num, alpha, spectrum, 1);
                    df = 0.5 * samplingRate / (double)spectralInfo.spectralLines;
                    double[] windowdata = new double[x.Length];
                    double CG = 0.0;
                    double ENBW = 0.0;
                    SeeSharpTools.JXI.SignalProcessing.Window.Window.GetWindow(windowType, ref windowdata, out CG, out ENBW);
                    UnitConversion(unitSetting: new UnitConvSetting(unit, PeakScaling.Rms, 50.0, PSD), spectrum: spectrum, df: df, spectrumType: SpectrumType.Amplitude, equivalentNoiseBw: ENBW);
                }

                void myBasicAnalysis(double[] timewaveform, double dt0, out double detectedFundamentalFreq, out double fundamentalPower, out double powerTotalHarmonic, out double noisePower, ref double[] mcomponentsLevel, int highestHarmonic = 10)
                {
                    double[] spectrum = new double[timewaveform.Length / 2];
                    SpectrumOutputUnit unit = SpectrumOutputUnit.V2;
                    WindowType windowType = WindowType.Four_Term_Nuttal;
                    int num = 9;
                    double CG = 0.0;
                    double ENBW = 0.0;
                    double[] windowdata = new double[timewaveform.Length];
                    SeeSharpTools.JXI.SignalProcessing.Window.Window.GetWindow(windowType, ref windowdata, out CG, out ENBW);
                    double maxValue = 0.0;
                    int num2 = 0;
                    int num3 = 0;
                    int num4 = spectrum.Length;
                    double num5 = 0.0;
                    double num6 = 0.0;
                    double num7 = 0.0;
                    if (mcomponentsLevel == null || mcomponentsLevel.Length != highestHarmonic)
                    {
                        mcomponentsLevel = new double[(highestHarmonic + 1 > 2) ? (highestHarmonic + 1) : 2];
                    }



                    Spectrum.PowerSpectrum(timewaveform, 1.0 / dt, ref spectrum, out var df, unit, windowType);
                    for (int ii = 0; ii < num / 2; ii++)
                    {
                        num7 += spectrum[ii];
                        spectrum[ii] = 0.0;
                    }

                    maxValue = -1.0;
                    maxValue = spectrum.Max();
                    num2 = Array.FindIndex(spectrum, (double s) => s == maxValue);
                    num3 = num2 - num / 2;
                    if (num3 < 0)
                    {
                        num3 = 0;
                    }

                    num4 = num3 + num;
                    if (num4 > spectrum.Length - 1)
                    {
                        num4 = spectrum.Length - 1;
                    }

                    for (int jj = num3; jj < num4; jj++)
                    {
                        num5 += spectrum[jj];
                        num6 += spectrum[jj] * (double)jj;
                        spectrum[jj] = 0.0;
                    }

                    detectedFundamentalFreq = num6 / num5 * df;
                    mcomponentsLevel[0] = num7 / ENBW;
                    mcomponentsLevel[1] = num5 / ENBW;
                    fundamentalPower = mcomponentsLevel[1];
                    powerTotalHarmonic = 0.0;
                    for (int j1 = 2; j1 <= highestHarmonic; j1++)
                    {
                        int num8 = (int)Math.Round(detectedFundamentalFreq / df * (double)j1 - 2.0);
                        if (num8 < 0)
                        {
                            num8 = 0;
                        }

                        num5 = 0.0;
                        for (num3 = 1; num3 < 5; num3++)
                        {
                            if (num8 + num3 < spectrum.Length)
                            {
                                num5 += spectrum[num8 + num3];
                                spectrum[num8 + num3] = 0.0;
                            }
                        }

                        mcomponentsLevel[j1] = num5 / ENBW;
                        powerTotalHarmonic += mcomponentsLevel[j1];
                    }

                    double num9 = 0.0;
                    for (int k = 0; k < spectrum.Length; k++)
                    {
                        num9 += spectrum[k];
                    }

                    noisePower = num9 / ENBW;
                    for (int jj = 1; jj < highestHarmonic + 1; jj++)
                    {
                        mcomponentsLevel[jj] = Math.Sqrt(mcomponentsLevel[jj] * 2.0);
                    }

                    mcomponentsLevel[0] = Math.Sqrt(mcomponentsLevel[0]);
                }
                */

                /*
                void PublicAnalysis(double[] timewaveform, double d_t, out double detectedFundamentalFreq, out double Sinad, out double Thd, ref double[] componentsLevel, int highestHarmonic = 10)
                { 
                    double fundamentalPower = 0.0;
                    double powerTotalHarmonic = 0.0;
                    double noisePower = 0.0;
                    BasicAnalysis(timewaveform, d_t, out detectedFundamentalFreq, out fundamentalPower, out powerTotalHarmonic, out noisePower, ref componentsLevel, highestHarmonic);

                    Sinad = fundamentalPower / (powerTotalHarmonic + noisePower);
                    Sinad = 10.0 * Math.Log10(Sinad);

                    Thd = fundamentalPower / powerTotalHarmonic;
                    Thd = 10.0 * Math.Log10(Thd);

                    THD = powerTotalHarmonic / fundamentalPower;
                    THD = Math.Sqrt(THD);
                }
                */

                HarmonicAnalysis.THDAnalysis(packed, dt, out fundamentalFreq, out thd, ref componentsLevel);
                HarmonicAnalysis.SINADAnalysis(packed, dt, out fundamentalFreq, out sinad, ref componentsLevel);

                
                thd = Math.Pow(thd, 2);
                thd = 1.0 / thd;
                thd = 10.0 * Math.Log10(thd);

                double s, t, n, snr;
                s = Math.Pow((sinad / 10.0), 10);
                s = 1.0 / s;
                t = Math.Pow((thd / 10.0), 10);
                t = 1.0 / t;
                n = 1.0 / (s - t);
                snr = 10.0 * Math.Log10(n);




                SINAD[j] = sinad;//
                finalSINAD = finalSINAD + SINAD[j];

                ENOB[j] = (sinad - 1.76) / 6.02;//
                finalENOB = finalENOB + ENOB[j];

                SNR[j] = snr;//
                finalSNR = finalSNR + SNR[j];

                THD[j] = thd;//
                finalTHD = finalTHD + THD[j];
            }
            finalENOB = finalENOB / 30;
            finalSINAD = finalSINAD / 30;
            finalSNR = finalSNR / 30;
            finalTHD = finalTHD / 30;
            

            label2.Text = "ENOB:" + finalENOB.ToString("F2");
            label3.Text = "SINAD:" + finalSINAD.ToString("F2");
            label4.Text = "SNR:" + finalSNR.ToString("F2");
            label5.Text = "THD:" + (finalTHD).ToString("F2");

            /*
            label2.Text = "ENOB:11.24";
            label3.Text = "SINAD:68.88";
            label4.Text = "SNR:69.34";
            label5.Text = "THD:78.81";
            */



            easyChart_readData1.Plot(unpacked[0]);



            /*byte[] readData = new byte[64];                                 //声明8位无符号整型数组

            FileStream F = new FileStream(filename, FileMode.Open, FileAccess.Read);

            int[] adc_data = new int[25000];                      //声明整型数组

            int i, index = 1;                                 //声明用于计数的变量

            int j = 0;                                           //j表示文件读取的位置

            for (i = 0; i < 25000; i++)
            {
                adc_data[i] = 0;
            }

            List<int> listADC = new List<int> { };                //声明一个集合，用于合并数组

            int l;                //读取的总字节数

            F.Position = 0;

            while (true)
            {
                F.Position = j;
                l = F.Read(readData, 0, 64);
                j = j + 64;

                if (l < 64)
                {
                    showLog("\n end of file\n");
                    break;
                }

                int adc_code1, adc_code2;

                if ((readData[0] >> 5) == 4)
                {
                    showLog("frame start ");
                }

                else if ((readData[0] >> 5) == 6)
                {
                    showLog("frame end_"+ index);

                    index = index + 1;
                }

                else
                {
                    for (i = 3; i < 33; i = i + 3)
                    {
                        adc_code1 = (readData[i - 1] << 4) + ((readData[i] & 0xF0) >> 4);

                        adc_code2 = ((readData[i] & 0x0F) << 8) + readData[i + 1];

                        //  adc_data=水平拼接adc-code1,adc-code2
                        listADC.Add(adc_code1);
                        listADC.Add(adc_code2);
                    }

                    for (i = 35; i < 64; i = i + 3)
                    {
                        adc_code1 = (readData[i - 1] << 4) + ((readData[i] & 0xF0) >> 4);

                        adc_code2 = ((readData[i] & 0x0F) << 8) + readData[i + 1];

                        listADC.Add(adc_code1);
                        listADC.Add(adc_code2);
                    }

                }
            }
            F.Close();
            adc_data = listADC.ToArray();

            double[] double_adc_data = new double[25000];

            for (i = 0; i < 25000; i++)
            {
                double_adc_data[i] = ((double)adc_data[i] - 2048) / 2048;
            }


            ToneAnalysisResult toneAnalysisResult = HarmonicAnalyzer.ToneAnalysis(double_adc_data, 1, 10, true);

            label2.Text = "ENOB:" + toneAnalysisResult.ENOB;
            label3.Text="SINAD:"+ toneAnalysisResult.SINAD;
            label4.Text="SNR:"+ toneAnalysisResult.SNR;
            label5.Text="THD:"+ toneAnalysisResult.THD;

            easyChart_readData1.Plot(double_adc_data);*/
        }
    }
    
}

