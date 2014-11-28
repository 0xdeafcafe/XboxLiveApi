using System;

namespace XboxLiveApi.Exceptions
{
	public class FriendlyException : Exception
	{
		public FriendlyException(string message)
			: base(message) { }
	}
}