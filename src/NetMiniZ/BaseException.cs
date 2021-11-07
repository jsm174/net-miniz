using System;

namespace NetMiniZ
{
	public abstract class BaseException : Exception
	{
		public string ComponentName { get; }
		public int Status { get; }

		public BaseException(string componentName, int status)
		{
			ComponentName = componentName;
			Status = status;
		}
	}
}
