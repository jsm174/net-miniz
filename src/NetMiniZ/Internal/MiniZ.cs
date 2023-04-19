using System;
using System.Runtime.InteropServices;
using NetMiniZ.Interop;

namespace NetMiniZ.Internal
{
	internal static unsafe class MiniZ
	{
		[DllImport(Libraries.MiniZ, CallingConvention = CallingConvention.Cdecl, SetLastError = false, EntryPoint = "wrapper_tdefl_compressor_size")]
		internal static extern int wrapper_tdefl_compressor_size();

		[DllImport(Libraries.MiniZ, CallingConvention = CallingConvention.Cdecl, SetLastError = false, EntryPoint = "wrapper_tdefl_init")]
		internal static extern int wrapper_tdefl_init(void* d, void* pPut_buf_func, void* pPut_buf_user, uint flags);

		[DllImport(Libraries.MiniZ, CallingConvention = CallingConvention.Cdecl, SetLastError = false, EntryPoint = "wrapper_tdefl_compress")]
		internal static extern int wrapper_tdefl_compress(void* d, void* pIn_buf, ref IntPtr pIn_buf_size, void* pOut_buf, ref IntPtr pOut_buf_size, int flush);

		[DllImport(Libraries.MiniZ, CallingConvention = CallingConvention.Cdecl, SetLastError = false, EntryPoint = "wrapper_tinfl_decompressor_size")]
		internal static extern int wrapper_tinfl_decompressor_size();

		[DllImport(Libraries.MiniZ, CallingConvention = CallingConvention.Cdecl, SetLastError = false, EntryPoint = "wrapper_tinfl_decompress")]
		internal static extern int wrapper_tinfl_decompress(void* r, void* pIn_buf_next, ref IntPtr pIn_buf_size, void* pOut_buf_start, void* pOut_buf_next, ref IntPtr pOut_buf_size, uint decomp_flags);

		[DllImport(Libraries.MiniZ, CallingConvention = CallingConvention.Cdecl, SetLastError = false, EntryPoint = "wrapper_mz_compress")]
		internal static extern int wrapper_mz_compress(byte[] pDest, ref IntPtr pDest_len, byte[] pSource, IntPtr source_len, int level);

		[DllImport(Libraries.MiniZ, CallingConvention = CallingConvention.Cdecl, SetLastError = false, EntryPoint = "wrapper_mz_uncompress")]
		internal static extern int wrapper_mz_uncompress(byte[] pDest, ref IntPtr pDest_len, byte[] pSource, IntPtr source_len);
    }
}
