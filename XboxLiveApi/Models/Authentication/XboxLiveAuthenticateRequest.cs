namespace XboxLiveApi.Models.Authentication
{
	public class XboxLiveAuthenticate
		: IXboxLiveRequestProperties
	{
		/// <summary>
		/// The method of authentication used in the request - generally `RPS`
		/// </summary>
		public string AuthMethod { get; set; }

		/// <summary>
		/// The section of the Xbox Live website you're authorizing from
		/// </summary>
		public string SiteName { get; set; }

		/// <summary>
		/// The ticket to authorize with - in the format of `t={windows_live-access_token}`
		/// </summary>
		public string RpsTicket { get; set; }
	}
}