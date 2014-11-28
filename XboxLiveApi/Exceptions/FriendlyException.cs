using System;

namespace XboxLiveApi.Exceptions
{
	public class FriendlyException : Exception
	{
		public FriendlyException(string message)
			: base(message) { }

		public FriendlyException(string message, Exception exception)
			: base(message) { PureException = exception; }

		public Exception PureException { get; private set; }
	}
}