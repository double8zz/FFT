using SeeSharpTools.JXI.SignalProcessing.SpectrumAnalysis.RFSASpectrum;
using SeeSharpTools.JXI.SignalProcessing.Window;
using SeeSharpTools.JY.DSP.SoundVibration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using SeeSharpTools.JXI.SignalProcessing.GeneralSpectrum1;


namespace test1.GeneralSpectrum
{
    public class HarmonicAnalyzer1
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
        public static ToneAnalysisResult1 ToneAnalysis(double[] timewaveform, double dt = 1.0, int highestHarmonic = 10, bool resultInDB = true)

        {
            //IL_006c: Unknown result type (might be due to invalid IL or missing references)
            //IL_008a: Unknown result type (might be due to invalid IL or missing references)
            ToneAnalysisResult1 toneAnalysisResult = new ToneAnalysisResult1();
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
            Spectrum.PowerSpectrum(timewaveform, 1.0 / dt, ref array, out num6, (SpectrumOutputUnit1)1, val, double.NaN, false);
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
                FieldInfo[] fields = typeof(ToneAnalysisResult1).GetFields();
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
            SpectrumOutputUnit1 val = (SpectrumOutputUnit1)1;
            WindowType val2 = (WindowType)8;
            double num = 2.63191;
            double maxValue = 0.0;
            int num2 = 0;
            int num3 = array.Length;
            double num4 = 0.0;
            double num5 = 0.0;
            double num6 = 0.0;
            double num7 = default(double);
            Spectrum.PowerSpectrum(timewaveform, 1.0 / dt, ref array, out num7, val, val2, double.NaN, false);
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

    //     Tone Analysis Result
    public class ToneAnalysisResult1
    {
        //
        // 摘要:
        //     THD
        public double THD;

        //
        // 摘要:
        //     THD + Noise
        public double THDplusN;

        //
        // 摘要:
        //     SINAD
        public double SINAD;

        //
        // 摘要:
        //     SNR
        public double SNR;

        //
        // 摘要:
        //     Noise Floor
        public double NoiseFloor;

        //
        // 摘要:
        //     Effictive number of bits
        public double ENOB;
    }

}
