using System;

namespace NetMiniZ
{
    public class DecompressException : BaseException
    {
        public DecompressException(string componentName, int status)
            : base(componentName, status)
        { }

        public override string Message
        {
            get
            {
                return String.Format("Decompression routine {0} failed with error code {1}.", ComponentName, Status);
            }
        }
    }
}
