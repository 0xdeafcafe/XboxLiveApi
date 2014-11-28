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
		/// Unique Session based Id used to authenticate requests made to Xbox Live
		/// </summary>
		[JsonProperty("uhs")]
		public string UserHeaderSession { get; set; }
	}
}