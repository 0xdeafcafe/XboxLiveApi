using System;

namespace XboxLiveApi.Console.Models
{
	public class Settings
	{
		public string AccessToken { get; set; }

		public string RefreshToken { get; set; }

		public DateTime ExpiresAt { get; set; }

		public string Scope { get; set; }

		public long UserId { get; set; }

		public bool HasExpired()
		{
			return ExpiresAt < DateTime.UtcNow;
		}
	}
}