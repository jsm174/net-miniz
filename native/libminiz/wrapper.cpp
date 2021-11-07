#pragma once

#include "miniz.h"

#ifdef _WIN32
   #define DLLEXPORT __declspec(dllexport)
#else
   #define DLLEXPORT	
#endif

extern "C" mz_uint32 DLLEXPORT wrapper_tdefl_compressor_size()
{
	return sizeof(tdefl_compressor);
}

extern "C" tdefl_status DLLEXPORT wrapper_tdefl_init(tdefl_compressor *d, tdefl_put_buf_func_ptr pPut_buf_func, void *pPut_buf_user, int flags)
{
	return tdefl_init(d, pPut_buf_func, pPut_buf_user, flags);
}

extern "C" tdefl_status DLLEXPORT wrapper_tdefl_compress(tdefl_compressor *d, const void *pIn_buf, size_t *pIn_buf_size, void *pOut_buf, size_t *pOut_buf_size, tdefl_flush flush)
{
	return tdefl_compress(d, pIn_buf, pIn_buf_size, pOut_buf, pOut_buf_size, flush);
}

extern "C" mz_uint32 DLLEXPORT wrapper_tinfl_decompressor_size()
{
	return sizeof(tinfl_decompressor);
}

extern "C" tinfl_status DLLEXPORT wrapper_tinfl_decompress(tinfl_decompressor *r, const mz_uint8 *pIn_buf_next, size_t *pIn_buf_size, mz_uint8 *pOut_buf_start, mz_uint8 *pOut_buf_next, size_t *pOut_buf_size, const mz_uint32 decomp_flags)
{
	return tinfl_decompress(r, pIn_buf_next, pIn_buf_size, pOut_buf_start, pOut_buf_next, pOut_buf_size, decomp_flags);
}

