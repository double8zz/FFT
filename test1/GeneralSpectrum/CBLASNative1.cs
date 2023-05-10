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
    public class CBLASNative1
    {
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int cblas_dcopy(int n, double[] X, int incX, double[] Y, int incY);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int cblas_dcopy(int n, IntPtr X, int incX, double[] Y, int incY);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int cblas_dcopy(int n, double[] X, int incX, IntPtr Y, int incY);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int cblas_dcopy(int n, IntPtr X, int incX, IntPtr Y, int incY);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int cblas_zcopy(int n, Complex[] X, int incX, Complex[] Y, int incY);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int cblas_dscal(int n, double alpha, double[] X, int incX);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int cblas_dscal(int n, double alpha, IntPtr X, int incX);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern double cblas_dasum(int n, double[] x, int incx);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern double cblas_idamin(int n, double[] x, int incx);
    }    
}
