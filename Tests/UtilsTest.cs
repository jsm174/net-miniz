using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;
using System.Linq;
using NetMiniZ;

namespace Tests
{
    [TestClass]
    public class UtilsTest
    {
        [TestMethod]
        [DeploymentItem("../../../../libminiz/build/lib")]
        public void CompressDecompress()
        {
            var utils = new NetMiniZUtils();

            using (var outStream = new FileStream("Test_compressed.png", FileMode.Create, FileAccess.Write, FileShare.Read))
                using (var inStream = new FileStream("Test.png", FileMode.Open, FileAccess.Read, FileShare.Read))
                    utils.Compress(inStream, outStream, 6);

            using (var outStream = new FileStream("Test_decompressed.png", FileMode.Create, FileAccess.Write, FileShare.Read))
                using (var inStream = new FileStream("Test_compressed.png", FileMode.Open, FileAccess.Read, FileShare.Read))
                    utils.Decompress(inStream, outStream);

            if (File.ReadAllBytes(new FileInfo("Test.png").FullName).SequenceEqual(
                 File.ReadAllBytes(new FileInfo("Test_decompressed.png").FullName)))
            {
                Console.WriteLine("Test_decompressed.png matches Test.png");
            }
            else
            {
                throw new Exception("Test_decompressed.png does not match Test.png");
            }
        }
    }
}
