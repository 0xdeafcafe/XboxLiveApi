using Newtonsoft.Json;

namespace XboxLiveApi.Models.Authentication
{
	public class XboxLiveAuthorizeResponse
		: IXboxLiveResponseDisplayClaims
	{
		/// <summary>
		/// Xbox User Interface Options
		/// </summary>
		public XboxLiveAuthorizeReponseOptions[] Xui { get; set; }
	}

	public class XboxLiveAuthorizeReponseOptions
	{
		/// <summary>
		/// Age Gate of the user
		/// </summary>
		[JsonProperty("agg")]
		public string AgeGate { get; set; }

		/// <summary>
		/// Gamertag of the user
		/// </summary>
		[JsonProperty("gtg")]
		public string Gamertag { get; set; }

		/// <summary>
		/// Some sort of private key for the user (unknown1)
		/// </summary>
		[JsonProperty("prv")]
		public string PrivateData { get; set; }

		/// <summary>
		/// Xbox Id of the user
		/// </summary>
		[JsonProperty("xid")]
		public long XboxUserId { get; set; }

		/// <summary>
		/// Unique Session based Id used to authenticate requests made to Xbox Live
		/// </summary>
		[JsonProperty("uhs")]
		public string UserHeaderSession { get; set; }
	}
}