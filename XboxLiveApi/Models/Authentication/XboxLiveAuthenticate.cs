namespace XboxLiveApi.Models.Authentication
{
	public class XboxLiveAuthenticate
	{
		public string TokenType { get; set; }

		public string RelyingParty { get; set; }

		public XboxLiveAuthenticateProperties Properties { get; set; }
	}

	public class XboxLiveAuthenticateProperties
	{
		public string AuthMethod { get; set; }

		public string SiteName { get; set; }

		public string RpsTicket { get; set; }
	}
}