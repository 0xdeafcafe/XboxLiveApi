using Newtonsoft.Json;
using System;

namespace XboxLiveApi.Models.Authentication
{
	public class WindowsLiveAuthenticationResponse
		: Result
	{
		[JsonProperty("access_token")]
		public string AccessToken { get; set; }

		[JsonProperty("refresh_token")]
		public string RefreshToken { get; set; }

		[JsonProperty("token_type")]
		public string TokenType { get; set; }

		[JsonProperty("expires_in")]
		public int ExpiresIn { get; set; }

		[JsonProperty("expires_at")]
		public DateTime ExpiresAt { get; set; }

		[JsonProperty("scope")]
		public string Scope { get; set; }

		[JsonProperty("user_id")]
		public long UserId { get; set; }
	}
}
