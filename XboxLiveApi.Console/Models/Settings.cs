using System;

namespace XboxLiveApi.Console.Models
{
	public class Settings
	{
		public string AccessToken { get; set; }

		public string RefreshToken { get; set; }

		public DateTime ExpiresAt { get; set; }

		public string Token { get; set; }

		public string UserHeaderSession { get; set; }

		public long XboxUserId { get; set; }

		public string Gamertag { get; set; }

		public string AgeGate { get; set; }

		public bool SettingsAreValid()
		{
			return (AccessToken != null && RefreshToken != null && Token != null && Gamertag != null && UserHeaderSession != null);
		}
	}
}