namespace NetMiniZ
{
	public class CompressException : BaseException
	{
		public CompressException(string componentName, int status) : base(componentName, status)
		{
		}

		public override string Message
		{
			get
			{
				return string.Format("Compression routine {0} failed with error code {1}.", ComponentName, Status);
			}
		}
	}
}
