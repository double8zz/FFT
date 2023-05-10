using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Reflection;

using SeeSharpTools.JXI.SignalProcessing.Window;
using SeeSharpTools.JXI.SignalProcessing.SpectrumAnalysis.RFSASpectrum;
using SeeSharpTools.JXI.SignalProcessing.GeneralSpectrum;

namespace exe_demo
{
    public partial class MainForm : Form
    {
        private string filename;
        private string last_flle_path;
                
        public MainForm()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
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

        // 事件委托（先声明委托类型，再声明事件是委托）（打开按钮的点击事件）的事件处理程序
        // 此处buttonOpenFile.Click为内置事件，无需事先定义
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
                        // l仅仅是用来检查数据帧头是否出错的，没有绘图的实际意义
                        l = G.ReadUInt32();          
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
                // 取每个pack的长度字段（在pack首部4B）
                readdata = G.ReadBytes(4);

                Array.Reverse(readdata);

                UInt32 tmplen;

                tmplen = BitConverter.ToUInt32(readdata, 0);

                n++;

                PACK pACK = new PACK();



                pACK.len = tmplen;

                byte[] temp;
                // 取每个pack的剩余字段（在pack首部后40B）
                temp = G.ReadBytes(40);
                // 网络发送为大端字节序->转成小端字节序（主机字节序；字节间序同，只需转各字节内存储序），且读的所有数据项都是4B的倍数，每次读4B的倍数即可
                Array.Reverse(temp);





                pACK.ch_id = BitConverter.ToUInt32(temp, 0);

                //pACK.rsv = BitConverter.ToUInt32(temp, 4);

                pACK.tof = BitConverter.ToUInt64(temp, 20);

                pACK.tg_id = BitConverter.ToUInt32(temp, 28);

                pACK.t0_id = BitConverter.ToUInt32(temp, 32);

                pACK.d_ty = BitConverter.ToUInt32(temp, 36);

                // 表示总共需要读取的字节数k（总长-非数据部分）
                k = (int)tmplen - 48;   
                // data数组元素大小修改为2B
                pACK.data = new UInt16[k / 2];

                for (p = 0; p < k / 2; p++)
                {
                    readdata = G.ReadBytes(2);

                    Array.Reverse(readdata);

                    pACK.data[p] = BitConverter.ToUInt16(readdata, 0);
                }


                packs.Add(pACK);

            }
        }

        public class ToneAnalysisResult
        {            
            //     THD
            public double THD;
                        
            //     THD + Noise
            public double THDplusN;

            //     SINAD
            public double SINAD;

            //     SNR
            public double SNR;

            //     Noise Floor
            public double NoiseFloor;

            //     Effictive number of bits
            public double ENOB;
        }

        public static class HarmonicAnalyzer
        {
            //
            // 摘要:
            //     Over view Tone Analysis
            //
            // 参数:
            //   timewaveform:
            //     Waveform in time space
            //
            //   dt:
            //     Time interval of waveform
            //
            //   highestHarmonic:
            //     HighestHamonic level
            //
            //   resultInDB:
            //     If return result in DB
            public static ToneAnalysisResult ToneAnalysis(double[] timewaveform, double dt = 1.0, int highestHarmonic = 10, bool resultInDB = true)
            {
                //IL_006c: Unknown result type (might be due to invalid IL or missing references)
                //IL_008a: Unknown result type (might be due to invalid IL or missing references)
                ToneAnalysisResult toneAnalysisResult = new ToneAnalysisResult();
                double[] componentsLevel = new double[highestHarmonic + 1];
                ToneAnalysis(timewaveform, dt, out var _, out var THD, ref componentsLevel, highestHarmonic);
                double num = 0.0;
                double num2 = 0.0;
                double num3 = 0.0;
                double num4 = 0.0;
                double num5 = 0.0;
                double num6 = 0.0;
                double[] array = new double[timewaveform.Length / 2];
                WindowType val = (WindowType)8;
                double num7 = 2.63191;
                Spectrum.PowerSpectrum(timewaveform, 1.0 / dt, ref array, ref num6, (SpectrumUnit)1, val, double.NaN, false);
                for (int i = 1; i < array.Length; i++)
                {
                    num += array[i];
                }

                num2 = Math.Sqrt(num / num7);
                num3 = Math.Sqrt(num2 * num2 - componentsLevel[1] * componentsLevel[1] / 2.0);
                num5 = num3 / num2;
                toneAnalysisResult.THD = THD;
                toneAnalysisResult.THDplusN = num5;
                toneAnalysisResult.SINAD = num2 / num3;
                num4 = num2 * num2 - componentsLevel[1] * componentsLevel[1] / 2.0;
                for (int j = 2; j < componentsLevel.Length; j++)
                {
                    num4 -= componentsLevel[j] * componentsLevel[j] / 2.0;
                }

                toneAnalysisResult.SNR = Math.Sqrt(componentsLevel[1] * componentsLevel[1] / 2.0 / num4);
                toneAnalysisResult.NoiseFloor = toneAnalysisResult.SNR * Math.Sqrt(timewaveform.Length / 2);
                toneAnalysisResult.ENOB = 1.0;
                if (resultInDB)
                {
                    FieldInfo[] fields = typeof(ToneAnalysisResult).GetFields();
                    foreach (FieldInfo fieldInfo in fields)
                    {
                        fieldInfo.SetValue(toneAnalysisResult, 20.0 * Math.Log10((double)fieldInfo.GetValue(toneAnalysisResult)));
                    }

                    toneAnalysisResult.ENOB = (toneAnalysisResult.SINAD - 1.76) / 6.02;
                }
                else
                {
                    toneAnalysisResult.ENOB = (20.0 * Math.Log10(toneAnalysisResult.SINAD) - 1.76) / 6.02;
                }

                return toneAnalysisResult;
            }

            //
            // 摘要:
            //     Tone analysis
            //
            // 参数:
            //   timewaveform:
            //     Waveform in time space
            //
            //   dt:
            //     Time interval of waveform
            //
            //   detectedFundamentalFreq:
            //     Fundamental frequency of waveform
            //
            //   THD:
            //     THD value of waveform
            //
            //   componentsLevel:
            //     The power of each harmonic wave.
            //
            //   highestHarmonic:
            //     The highest level of harmonic level that will be calculated.
            public static void ToneAnalysis(double[] timewaveform, double dt, out double detectedFundamentalFreq, out double THD, ref double[] componentsLevel, int highestHarmonic = 10)
            {
                //IL_0012: Unknown result type (might be due to invalid IL or missing references)
                //IL_0014: Unknown result type (might be due to invalid IL or missing references)
                //IL_0069: Unknown result type (might be due to invalid IL or missing references)
                //IL_006a: Unknown result type (might be due to invalid IL or missing references)
                double[] array = new double[timewaveform.Length / 2];
                SpectrumUnit val = (SpectrumUnit)1;
                WindowType val2 = (WindowType)8;
                double num = 2.63191;
                double maxValue = 0.0;
                int num2 = 0;
                int num3 = array.Length;
                double num4 = 0.0;
                double num5 = 0.0;
                double num6 = 0.0;
                double num7 = default(double);
                Spectrum.PowerSpectrum(timewaveform, 1.0 / dt, ref array, ref num7, val, val2, double.NaN, false);
                maxValue = -1.0;
                maxValue = array.Max();
                num2 = Array.FindIndex(array, (double s) => s == maxValue) - 7;
                if (num2 < 0)
                {
                    num2 = 0;
                }

                num3 = num2 + 14;
                if (num3 > array.Length - 1)
                {
                    num3 = array.Length - 1;
                }

                for (int i = num2; i < num3; i++)
                {
                    num4 += array[i];
                    num5 += array[i] * (double)i;
                }

                detectedFundamentalFreq = num5 / num4 * num7;
                componentsLevel[0] = array[0] / num;
                componentsLevel[1] = num4 / num;
                num6 = 0.0;
                for (int i = 2; i <= highestHarmonic; i++)
                {
                    int num8 = (int)Math.Round(detectedFundamentalFreq / num7 * (double)i - 7.0);
                    if (num8 < 0)
                    {
                        num8 = 0;
                    }

                    num4 = 0.0;
                    for (num2 = 0; num2 < 15; num2++)
                    {
                        if (num8 + num2 < array.Length)
                        {
                            num4 += array[num8 + num2];
                        }
                    }

                    componentsLevel[i] = num4 / num;
                    num6 += componentsLevel[i];
                }

                THD = num6 / componentsLevel[1];
                THD = Math.Sqrt(THD);
                for (int i = 0; i <= highestHarmonic; i++)
                {
                    componentsLevel[i] = Math.Sqrt(componentsLevel[i] * 2.0);
                }
            }
        }
    }
}
