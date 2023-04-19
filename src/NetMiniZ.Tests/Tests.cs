using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;
using System.Linq;

namespace NetMiniZ
{
	[TestClass]
	public class Tests
	{
		[TestMethod]
		public void CompressDecompress()
		{
			using (var outStream = new FileStream("Test_compressed.png", FileMode.Create, FileAccess.Write, FileShare.Read))
			using (var inStream = new FileStream("Test.png", FileMode.Open, FileAccess.Read, FileShare.Read))
				NetMiniZ.Compress(inStream, outStream, 6);

			using (var outStream = new FileStream("Test_decompressed.png", FileMode.Create, FileAccess.Write, FileShare.Read))
			using (var inStream = new FileStream("Test_compressed.png", FileMode.Open, FileAccess.Read, FileShare.Read))
				NetMiniZ.Decompress(inStream, outStream);

			if (File.ReadAllBytes(new FileInfo("Test.png").FullName).SequenceEqual(
				 File.ReadAllBytes(new FileInfo("Test_decompressed.png").FullName)))
				Console.WriteLine("Test_decompressed.png matches Test.png");
			else
				throw new Exception("Test_decompressed.png does not match Test.png");
		}

		[TestMethod]
		public void MZCompressUncompress()
		{
			byte[] fileBytes = File.ReadAllBytes("Test.png");
			NetMiniZ.MZCompress(fileBytes, out byte[] dest);
			File.WriteAllBytes("Test_mz_compressed.png", dest);

			fileBytes = File.ReadAllBytes("Test_mz_compressed.png");
			NetMiniZ.MZUncompress(fileBytes, out dest);
			File.WriteAllBytes("Test_mz_uncompressed.png", dest);

			if (File.ReadAllBytes(new FileInfo("Test.png").FullName).SequenceEqual(
				File.ReadAllBytes(new FileInfo("Test_mz_uncompressed.png").FullName)))
				Console.WriteLine("Test_mz_uncompressed.png matches Test.png");
			else
				throw new Exception("Test_mz_uncompressed.png does not match Test.png");
		}
	}
}
