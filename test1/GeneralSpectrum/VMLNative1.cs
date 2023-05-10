using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;
using System.Runtime.InteropServices;
using System.Security;

namespace test1.GeneralSpectrum
{
    [SuppressUnmanagedCodeSecurity]
    public class VMLNative1
    {
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdAtan2(int n, double[] a, double[] b, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdAdd(int n, double[] a, double[] b, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdAdd(int n, IntPtr a, IntPtr b, IntPtr y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdSub(int n, double[] a, double[] b, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdSub(int n, IntPtr a, IntPtr b, IntPtr y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdSqr(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdMul(int n, double[] a, double[] b, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdMul(int n, double[] a, IntPtr b, IntPtr y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdMul(int n, double[] a, IntPtr b, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdAbs(int n, double[] a, double[] r);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdInv(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdDiv(int n, double[] a, double[] b, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdSqrt(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdPowx(int n, double[] a, double b, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdExp(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdExpm1(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdLn(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdLog10(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdLog1p(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdCos(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdSin(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdTan(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdCosh(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdSinh(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdTanh(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdAcosh(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdAsinh(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdAtanh(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdFloor(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdCeil(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdTrunc(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdRound(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdNearbyInt(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int Rint(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdModf(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vdFrac(int n, double[] a, double[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vsAbs(int n, float[] a, float[] r);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vzAdd(int n, Complex[] a, Complex[] b, Complex[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vsLog10(int n, float[] a, float[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vzLog10(int n, Complex[] a, Complex[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vsAdd(int n, float[] a, float[] b, float[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vsMul(int n, float[] a, float[] b, float[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vzMul(int n, Complex[] a, Complex[] b, Complex[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vzAbs(int n, Complex[] a, double[] r);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vsSqr(int n, float[] a, float[] y);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int vzSqr(int n, Complex[] a, Complex[] y);
    }
}
