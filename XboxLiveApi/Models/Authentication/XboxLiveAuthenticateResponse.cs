using Newtonsoft.Json;

namespace XboxLiveApi.Models.Authentication
{
	public class XboxLiveAuthenticateResponse
		: IXboxLiveResponseDisplayClaims
	{
		/// <summary>
		/// Xbox User Interface Options
		/// </summary>
		[JsonProperty("xui")]
		public XboxLiveAuthorizeRequestOptions[] Xui { get; set; }
	}

	public class XboxLiveAuthorizeRequestOptions
	{
		/// <summary>
		/// No fucking clue here (unknown1)
		/// </summary>
		[JsonProperty("uhs")]
		public string Uhs { get; set; }
	}
}