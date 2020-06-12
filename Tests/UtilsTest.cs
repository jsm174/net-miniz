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

            var outStream = new FileStream("Test_compressed.png", FileMode.Create, FileAccess.Write, FileShare.Read);
            var inStream = new FileStream("Test.png", FileMode.Open, FileAccess.Read, FileShare.Read);
            utils.Compress(inStream, outStream, 6);

            outStream = new FileStream("Test_decompressed.png", FileMode.Create, FileAccess.Write, FileShare.Read);
            inStream = new FileStream("Test_compressed.png", FileMode.Open, FileAccess.Read, FileShare.Read);
            utils.Decompress(inStream, outStream);

            var s1Info = new FileInfo("Test.png");
            var s2Info = new FileInfo("Test_decompressed.png");

            if (!File.ReadAllBytes(s1Info.FullName).SequenceEqual(
                 File.ReadAllBytes(s2Info.FullName)))
            {
                throw new Exception("Test_decompressed.png does not match Test.png");
            }
        }
    }
}
