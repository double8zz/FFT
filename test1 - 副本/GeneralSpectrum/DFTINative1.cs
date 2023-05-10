using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.Security;

namespace test1.GeneralSpectrum
{
    [SuppressUnmanagedCodeSecurity]
    public class DFTINative1
    {
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int DftiCreateDescriptor(ref IntPtr desc, int precision, int domain, int dimention, int length);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int DftiCommitDescriptor(IntPtr desc);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int DftiFreeDescriptor(ref IntPtr desc);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int DftiSetValue(IntPtr desc, int config_param, int config_val);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int DftiSetValue(IntPtr desc, int config_param, double config_val);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int DftiGetValue(IntPtr desc, int config_param, ref double config_val);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int DftiComputeForward(IntPtr desc, [In] double[] x_in, [Out] double[] x_out);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int DftiComputeForward(IntPtr desc, [In] IntPtr x_in, [Out] IntPtr x_out);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int DftiComputeForward(IntPtr desc, [In] double[] x_in, [Out] IntPtr x_out);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int DftiComputeForward(IntPtr desc, [In] IntPtr x_in, [Out] double[] x_out);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int DftiComputeBackward(IntPtr desc, [In] double[] x_in, [Out] double[] x_out);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int DftiComputeBackward(IntPtr desc, [In] IntPtr x_in, [Out] IntPtr x_out);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int DftiComputeBackward(IntPtr desc, [In] double[] x_in, [Out] IntPtr x_out);

        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int DftiComputeBackward(IntPtr desc, [In] IntPtr x_in, [Out] double[] x_out);
    }
}
