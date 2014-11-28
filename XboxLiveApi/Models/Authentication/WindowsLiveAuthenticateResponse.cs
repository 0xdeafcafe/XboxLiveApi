using Newtonsoft.Json;

namespace XboxLiveApi.Models.Authentication
{
	public class WindowsLiveAuthenticateResponse
		: IWindowsLiveResponseResult
	{
		/// <summary>
		/// The generated Access Token
		/// </summary>
		[JsonProperty("access_token")]
		public string AccessToken { get; set; }

		/// <summary>
		/// The refresh token used to generate a new Access Token
		/// </summary>
		[JsonProperty("refresh_token")]
		public string RefreshToken { get; set; }

		/// <summary>
		/// The type of Access Token returned
		/// </summary>
		[JsonProperty("token_type")]
		public string TokenType { get; set; }

		/// <summary>
		/// How many seconds until the Access Token expires
		/// </summary>
		[JsonProperty("expires_in")]
		public int ExpiresIn { get; set; }

		/// <summary>
		/// The scope of the permissions the Access Token provides
		/// </summary>
		[JsonProperty("scope")]
		public string Scope { get; set; }

		/// <summary>
		/// The Id of the user that the Access Token belongs to
		/// </summary>
		[JsonProperty("user_id")]
		public string UserId { get; set; }
	}
}
