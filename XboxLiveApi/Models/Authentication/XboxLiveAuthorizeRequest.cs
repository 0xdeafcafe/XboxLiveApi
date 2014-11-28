namespace XboxLiveApi.Models.Authentication
{
	public class XboxLiveAuthorize
		: IXboxLiveRequestProperties
	{
		/// <summary>
		/// The Id of the Xbox Live Sandbox the user is authenticating too.
		/// </summary>
		public string SandboxId { get; set; }

		/// <summary>
		/// A list of user tokens - Only experimented with sending 1 token
		/// </summary>
		public string[] UserTokens { get; set; }
	}
}