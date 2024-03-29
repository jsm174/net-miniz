﻿using System;
using System.IO;
using NetMiniZ.Internal;

namespace NetMiniZ
{
	unsafe public static class NetMiniZ
	{
		private static readonly uint[] s_tdefl_num_probes = new uint[11] { 0, 1, 6, 32, 16, 32, 128, 256, 512, 768, 1500 };

		// IN_BUF_SIZE is the size of the file read buffer.
		// IN_BUF_SIZE must be >= 1
		private const int IN_BUF_SIZE = 1024 * 512;

		// COMP_OUT_BUF_SIZE is the size of the output buffer used during compression.
		// COMP_OUT_BUF_SIZE must be >= 1 and <= OutBugSize
		private const int COMP_OUT_BUF_SIZE = 1024 * 512;

		// OUT_BUF_SIZE is the size of the output buffer used during decompression.
		// OUT_BUF_SIZE must be a power of 2 >= TINFL_LZ_DICT_SIZE (because the low-level decompressor not only writes, but reads from the output buffer as it decompresses)
		//#define OutBugSize (TINFL_LZ_DICT_SIZE)
		private const int OUT_BUF_SIZE = 1024 * 512;

		private const uint TDEFL_WRITE_ZLIB_HEADER = 0x01000;
		private const uint TDEFL_GREEDY_PARSING_FLAG = 0x04000;
		private const uint TDEFL_FORCE_ALL_RAW_BLOCKS = 0x80000;

		private const int TDEFL_STATUS_OKAY = 0;
		private const int TDEFL_STATUS_DONE = 1;

		private const int TDEFL_NO_FLUSH = 0;
		private const int TDEFL_FINISH = 4;

		private const uint TINFL_FLAG_PARSE_ZLIB_HEADER = 1;
		private const uint TINFL_FLAG_HAS_MORE_INPUT = 2;
		private const uint TINFL_FLAG_COMPUTE_ADLER32 = 8;

		private const int TINFL_STATUS_DONE = 0;

		public static void Compress(Stream inputStream, Stream outputStream, int compressionLevel)
		{
			if (compressionLevel < 0)
				compressionLevel = 0;
			if (compressionLevel > 10)
				compressionLevel = 10;

			// create tdefl() compatible flags (we have to compose the low-level flags ourselves, or use tdefl_create_comp_flags_from_zip_params() but that means MINIZ_NO_ZLIB_APIS can't be defined).
			uint comp_flags = TDEFL_WRITE_ZLIB_HEADER | s_tdefl_num_probes[compressionLevel] | ((compressionLevel <= 3) ? TDEFL_GREEDY_PARSING_FLAG : 0);
			if (compressionLevel <= 0)
				comp_flags |= TDEFL_FORCE_ALL_RAW_BLOCKS;

			// Initialize the low-level compressor.
			void* g_deflator = stackalloc byte[MiniZ.wrapper_tdefl_compressor_size()];
			{
				int status = MiniZ.wrapper_tdefl_init(g_deflator, null, null, comp_flags);
				if (status != 0)
					throw new CompressException("tdefl_init", status);
			}

			// Initialize buffers
			byte[] s_inbufGC = new byte[IN_BUF_SIZE];
			byte[] s_outbufGC = new byte[OUT_BUF_SIZE];

			fixed (byte* s_inbuf = s_inbufGC)
			fixed (byte* s_outbuf = s_outbufGC)
			{

				// Compression
				int avail_in = 0;
				int avail_out = COMP_OUT_BUF_SIZE;
				int total_in = 0;
				int total_out = 0;
				byte* next_in = s_inbuf; // const ptr
				byte* next_out = s_outbuf;
				bool flush = false;
				do
				{
					if ((avail_in == 0) && !flush)
					{
						next_in = s_inbuf;
						avail_in = inputStream.Read(s_inbufGC, 0, IN_BUF_SIZE);

						flush = avail_in < IN_BUF_SIZE; //Detect EOF
					}

					IntPtr in_bytes = new IntPtr(avail_in);
					IntPtr out_bytes = new IntPtr(avail_out);

					// Compress as much of the input as possible (or all of it) to the output buffer.
					int status = MiniZ.wrapper_tdefl_compress(g_deflator, next_in, ref in_bytes, next_out, ref out_bytes,
						flush ? TDEFL_FINISH : TDEFL_NO_FLUSH);

					int in_bytes32 = in_bytes.ToInt32();
					int out_bytes32 = out_bytes.ToInt32();

					next_in += in_bytes32;
					avail_in -= in_bytes32;
					total_in += in_bytes32;

					next_out += out_bytes32;
					avail_out -= out_bytes32;
					total_out += out_bytes32;

					if ((status != TDEFL_STATUS_OKAY) || (avail_out <= 0))
					{
						// Output buffer is full, or compression is done or failed, so write buffer to output file.
						outputStream.Write(s_outbufGC, 0, COMP_OUT_BUF_SIZE - avail_out);

						next_out = s_outbuf;
						avail_out = COMP_OUT_BUF_SIZE;
					}

					if (status == TDEFL_STATUS_DONE)
						break;
					else if (status != TDEFL_STATUS_OKAY)
						throw new CompressException("tdefl_compress", status);
				} while (true);
			}
		}

		public static void Decompress(Stream inputStream, Stream outputStream)
		{
			// Initialize decompressor
			void* inflator = stackalloc byte[MiniZ.wrapper_tinfl_decompressor_size()];
			*((ulong*)(inflator)) = 0; //tinfl_init(inflator);

			// Initialize buffers
			byte[] s_inbufGC = new byte[IN_BUF_SIZE];
			byte[] s_outbufGC = new byte[OUT_BUF_SIZE];

			fixed (byte* s_inbuf = s_inbufGC)
			fixed (byte* s_outbuf = s_outbufGC)
			{

				// Decompress
				int avail_in = 0;
				int avail_out = COMP_OUT_BUF_SIZE;
				int total_in = 0;
				int total_out = 0;
				byte* next_in = s_inbuf; // const ptr
				byte* next_out = s_outbuf;
				bool flush = false;
				do
				{
					if ((avail_in == 0) && !flush)
					{
						// Input buffer is empty, so read more bytes from input file.
						next_in = s_inbuf;
						avail_in = inputStream.Read(s_inbufGC, 0, IN_BUF_SIZE);

						flush = avail_in < IN_BUF_SIZE;
					}

					IntPtr in_bytes = new IntPtr(avail_in);
					IntPtr out_bytes = new IntPtr(avail_out);

					int status = MiniZ.wrapper_tinfl_decompress(inflator, next_in, ref in_bytes, s_outbuf, next_out, ref out_bytes,
						(flush ? 0 : TINFL_FLAG_HAS_MORE_INPUT) | TINFL_FLAG_PARSE_ZLIB_HEADER | TINFL_FLAG_COMPUTE_ADLER32);

					int in_bytes32 = in_bytes.ToInt32();
					int out_bytes32 = out_bytes.ToInt32();

					avail_in -= in_bytes32;
					next_in += in_bytes32;
					total_in += in_bytes32;

					avail_out -= out_bytes32;
					next_out += out_bytes32;
					total_out += out_bytes32;

					if ((status <= TINFL_STATUS_DONE) || (avail_out <= 0))
					{
						// Output buffer is full, or decompression is done, so write buffer to output file.
						outputStream.Write(s_outbufGC, 0, OUT_BUF_SIZE - avail_out);

						next_out = s_outbuf;
						avail_out = OUT_BUF_SIZE;
					}

					// If status is <= TINFL_STATUS_DONE then either decompression is done or something went wrong.
					if (status <= TINFL_STATUS_DONE)
						if (status == TINFL_STATUS_DONE)
							break;
						else
							throw new DecompressException("tinfl_decompress", status);
				} while (true);
			}
		}

		public enum MZStatus
		{
			OK = 0,
			STREAM_END = 1,
			NEED_DICT = 2,
			ERRNO = -1,
			STREAM_ERROR = -2,
			DATA_ERROR = -3,
			MEM_ERROR = -4,
			BUF_ERROR = -5,
			VERSION_ERROR = -6,
			PARAM_ERROR = -10000
		}

		public enum MZCompressionLevel
		{
			NO_COMPRESSION = 0,
			BEST_SPEED = 1,
			BEST_COMPRESSION = 9,
			UBER_COMPRESSION = 10,
			DEFAULT_LEVEL = 6,
			DEFAULT_COMPRESSION = -1
		}

		public static int MZCompress(byte[] source, out byte[] dest, MZCompressionLevel level = MZCompressionLevel.DEFAULT_COMPRESSION)
		{
			int sourceLen = source.Length;
			IntPtr sourceLenPtr = new IntPtr(sourceLen);

			int destLen = sourceLen;

			int result;
			do
			{
				IntPtr destLenPtr = new IntPtr(destLen);
				dest = new byte[destLen];

				result = MiniZ.wrapper_mz_compress(dest, ref destLenPtr, source, sourceLenPtr, (int)level);

				if (result == 0)
				{
					Array.Resize(ref dest, destLenPtr.ToInt32());
					return result;
				}
				else if (result == (int)MZStatus.BUF_ERROR)
					destLen *= 2;
				else
				{
					dest = null;
					return result;
				}
			} while (true);
		}

		public static int MZUncompress(byte[] source, out byte[] dest)
		{
			int sourceLen = source.Length;
			IntPtr sourceLenPtr = new IntPtr(sourceLen);

			int destLen = sourceLen * 2;

			int result;
			do
			{
				IntPtr destLenPtr = new IntPtr(destLen);
				dest = new byte[destLen];

				result = MiniZ.wrapper_mz_uncompress(dest, ref destLenPtr, source, sourceLenPtr);

				if (result == 0)
				{
					Array.Resize(ref dest, destLenPtr.ToInt32());
					return result;
				}
				else if (result == (int)MZStatus.BUF_ERROR)
					destLen *= 2;
				else
				{
					dest = null;
					return result;
				}
			} while (true);
		}
	}
}
