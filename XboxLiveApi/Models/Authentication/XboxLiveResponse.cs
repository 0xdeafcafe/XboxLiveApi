using System;

namespace XboxLiveApi.Models.Authentication
{
	public class XboxLiveResponse<T>
		where T : IXboxLiveResponseDisplayClaims
	{
		/// <summary>
		/// The exact time the token was issued
		/// </summary>
		public DateTime IssueInstant { get; set; }

		/// <summary>
		/// The token will not be issued after this time
		/// </summary>
		public DateTime NotAfter { get; set; }

		/// <summary>
		/// The token issued by Xbox Live
		/// </summary>
		public string Token { get; set; }

		/// <summary>
		/// DisplayClaims  of the Xbox Live Response
		/// </summary>
		public T DisplayClaims { get; set; }
	}

	public interface IXboxLiveResponseDisplayClaims { }
}