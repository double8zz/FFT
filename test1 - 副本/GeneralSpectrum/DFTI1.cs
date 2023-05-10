using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace test1.GeneralSpectrum
{
    public class DFTI1
    {
        public static int PRECISION = 3;

        public static int FORWARD_DOMAIN = 0;

        public static int DIMENSION = 1;

        public static int LENGTHS = 2;

        public static int NUMBER_OF_TRANSFORMS = 7;

        public static int FORWARD_SCALE = 4;

        public static int BACKWARD_SCALE = 5;

        public static int PLACEMENT = 11;

        public static int COMPLEX_STORAGE = 8;

        public static int REAL_STORAGE = 9;

        public static int CONJUGATE_EVEN_STORAGE = 10;

        public static int DESCRIPTOR_NAME = 20;

        public static int PACKED_FORMAT = 21;

        public static int NUMBER_OF_USER_THREADS = 26;

        public static int INPUT_DISTANCE = 14;

        public static int OUTPUT_DISTANCE = 15;

        public static int INPUT_STRIDES = 12;

        public static int OUTPUT_STRIDES = 13;

        public static int ORDERING = 18;

        public static int TRANSPOSE = 19;

        public static int COMMIT_STATUS = 22;

        public static int VERSION = 23;

        public static int SINGLE = 35;

        public static int DOUBLE = 36;

        public static int COMPLEX = 32;

        public static int REAL = 33;

        public static int INPLACE = 43;

        public static int NOT_INPLACE = 44;

        public static int COMPLEX_COMPLEX = 39;

        public static int REAL_REAL = 42;

        public static int COMPLEX_REAL = 40;

        public static int REAL_COMPLEX = 41;

        public static int COMMITTED = 30;

        public static int UNCOMMITTED = 31;

        public static int ORDERED = 48;

        public static int BACKWARD_SCRAMBLED = 49;

        public static int NONE = 53;

        public static int CCS_FORMAT = 54;

        public static int PACK_FORMAT = 55;

        public static int PERM_FORMAT = 56;

        public static int CCE_FORMAT = 57;

        public static int VERSION_LENGTH = 198;

        public static int MAX_NAME_LENGTH = 10;

        public static int MAX_MESSAGE_LENGTH = 40;

        public static int NO_ERROR = 0;

        public static int MEMORY_ERROR = 1;

        public static int INVALID_CONFIGURATION = 2;

        public static int INCONSISTENT_CONFIGURATION = 3;

        public static int NUMBER_OF_THREADS_ERROR = 8;

        public static int MULTITHREADED_ERROR = 4;

        public static int BAD_DESCRIPTOR = 5;

        public static int UNIMPLEMENTED = 6;

        public static int MKL_INTERNAL_ERROR = 7;

        public static int LENGTH_EXCEEDS_INT32 = 9;

        public static int DftiCreateDescriptor(ref IntPtr desc, int precision, int domain, int dimention, int length)
        {
            return DFTINative1.DftiCreateDescriptor(ref desc, precision, domain, dimention, length);
        }

        public static int DftiFreeDescriptor(ref IntPtr desc)
        {
            return DFTINative1.DftiFreeDescriptor(ref desc);
        }

        public static int DftiSetValue(IntPtr desc, int configParam, int configVal)
        {
            return DFTINative1.DftiSetValue(desc, configParam, configVal);
        }

        public static int DftiSetValue(IntPtr desc, int configParam, double configVal)
        {
            return DFTINative1.DftiSetValue(desc, configParam, configVal);
        }

        public static int DftiGetValue(IntPtr desc, int configParam, ref double configVal)
        {
            return DFTINative1.DftiGetValue(desc, configParam, ref configVal);
        }

        public static int DftiCommitDescriptor(IntPtr desc)
        {
            return DFTINative1.DftiCommitDescriptor(desc);
        }

        public static int DftiComputeForward(IntPtr desc, [In] double[] x_in, [Out] double[] x_out)
        {
            return DFTINative1.DftiComputeForward(desc, x_in, x_out);
        }

        public static int DftiComputeForward(IntPtr desc, [In] IntPtr x_in, [Out] IntPtr x_out)
        {
            return DFTINative1.DftiComputeForward(desc, x_in, x_out);
        }

        public static int DftiComputeForward(IntPtr desc, [In] double[] x_in, [Out] IntPtr x_out)
        {
            return DFTINative1.DftiComputeForward(desc, x_in, x_out);
        }

        public static int DftiComputeForward(IntPtr desc, [In] IntPtr x_in, [Out] double[] x_out)
        {
            return DFTINative1.DftiComputeForward(desc, x_in, x_out);
        }

        public static int DftiComputeBackward(IntPtr desc, [In] double[] x_in, [Out] double[] x_out)
        {
            return DFTINative1.DftiComputeBackward(desc, x_in, x_out);
        }

        public static int DftiComputeBackward(IntPtr desc, [In] IntPtr x_in, [Out] IntPtr x_out)
        {
            return DFTINative1.DftiComputeBackward(desc, x_in, x_out);
        }

        public static int DftiComputeBackward(IntPtr desc, [In] double[] x_in, [Out] IntPtr x_out)
        {
            return DFTINative1.DftiComputeBackward(desc, x_in, x_out);
        }

        public static int DftiComputeBackward(IntPtr desc, [In] IntPtr x_in, [Out] double[] x_out)
        {
            return DFTINative1.DftiComputeBackward(desc, x_in, x_out);
        }
    }
}
