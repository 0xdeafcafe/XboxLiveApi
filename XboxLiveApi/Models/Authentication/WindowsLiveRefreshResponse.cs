using Newtonsoft.Json;

namespace XboxLiveApi.Models.Authentication
{
	public class WindowsLiveRefreshResponse
		: WindowsLiveAuthenticateResponse
	{
		/// <summary>
		/// The Safe Error Name
		/// </summary>
		[JsonProperty("error")]
		public string Error { get; set; }

		/// <summary>
		/// The Friendly Error Name
		/// </summary>
		[JsonProperty("error_description")]
		public string ErrorDescription { get; set; }
	}
}
