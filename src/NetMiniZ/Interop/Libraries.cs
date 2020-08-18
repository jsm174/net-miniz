using System;

namespace NetMiniZ.Interop
{
    public abstract unsafe class Libraries
    {
        public abstract int tdefl_compressor_size();
        public abstract int tdefl_init(void* d, void* pPut_buf_func, void* pPut_buf_user, uint flags);
        public abstract int tdefl_compress(void* d, void* pIn_buf, ref IntPtr pIn_buf_size, void* pOut_buf, ref IntPtr pOut_buf_size, int flush);
        public abstract int tinfl_decompressor_size();
        public abstract int tinfl_decompress(void* r, void* pIn_buf_next, ref IntPtr pIn_buf_size, void* pOut_buf_start, void* pOut_buf_next, ref IntPtr pOut_buf_size, uint decomp_flags);
    }
}
