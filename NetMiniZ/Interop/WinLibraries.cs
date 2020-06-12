using System;
using System.Runtime.InteropServices;

namespace NetMiniZ.Interop
{
    public unsafe class WinLibraries : Libraries
    {
        private const string lib = "libminiz-2.1.0.dll";

        [DllImport(lib, CallingConvention = CallingConvention.Cdecl, SetLastError = false, EntryPoint = "wrapper_tdefl_compressor_size")]
        private static extern int wrapper_tdefl_compressor_size();

        [DllImport(lib, CallingConvention = CallingConvention.Cdecl, SetLastError = false, EntryPoint = "wrapper_tdefl_init")]
        private static extern int wrapper_tdefl_init(void* d, void* pPut_buf_func, void* pPut_buf_user, uint flags);

        [DllImport(lib, CallingConvention = CallingConvention.Cdecl, SetLastError = false, EntryPoint = "wrapper_tdefl_compress")]
        private static extern int wrapper_tdefl_compress(void* d, void* pIn_buf, ref IntPtr pIn_buf_size, void* pOut_buf, ref IntPtr pOut_buf_size, int flush);

        [DllImport(lib, CallingConvention = CallingConvention.Cdecl, SetLastError = false, EntryPoint = "wrapper_tinfl_decompressor_size")]
        private static extern int wrapper_tinfl_decompressor_size();
      
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl, SetLastError = false, EntryPoint = "wrapper_tinfl_decompress")]
        private static extern int wrapper_tinfl_decompress(void* r, void* pIn_buf_next, ref IntPtr pIn_buf_size, void* pOut_buf_start, void* pOut_buf_next, ref IntPtr pOut_buf_size, uint decomp_flags);

        public override int tdefl_compressor_size()
        {
            return wrapper_tdefl_compressor_size();
        }

        public override int tdefl_init(void* d, void* pPut_buf_func, void* pPut_buf_user, uint flags)
        {
            return wrapper_tdefl_init(d, pPut_buf_func, pPut_buf_user, flags);
        }

        public override int tdefl_compress(void* d, void* pIn_buf, ref IntPtr pIn_buf_size, void* pOut_buf, ref IntPtr pOut_buf_size, int flush)
        {
            return wrapper_tdefl_compress(d, pIn_buf, ref pIn_buf_size, pOut_buf, ref pOut_buf_size, flush);
        }

        public override int tinfl_decompressor_size()
        {
            return wrapper_tinfl_decompressor_size();
        }

        public override int tinfl_decompress(void* r, void* pIn_buf_next, ref IntPtr pIn_buf_size, void* pOut_buf_start, void* pOut_buf_next, ref IntPtr pOut_buf_size, uint decomp_flags)
        {
            return wrapper_tinfl_decompress(r, pIn_buf_next, ref pIn_buf_size, pOut_buf_start, pOut_buf_next, ref pOut_buf_size, decomp_flags);
        }
    }
}
