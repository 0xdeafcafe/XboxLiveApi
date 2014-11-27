using System;

namespace XboxLiveAuthApi.Models
{
	public class Authentication 
		: Result
	{
		public string AccessToken { get; set; }

		public string RefreshToken { get; set; }

		public string TokenType { get; set; }

		public int ExpiresIn { get; set; }

		public DateTime ExpiresAt { get; set; }

		public string Scope { get; set; }

		public long UserId { get; set; }
	}
}