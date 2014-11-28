using Newtonsoft.Json;

namespace XboxLiveApi.Models.Authentication
{
	public class WindowsLiveRequest
	{
		/// <summary>
		/// The email address of the Microsoft Account to authenticate.
		/// </summary>
		[JsonProperty("identity")]
		public string Identity { get; set; }

		/// <summary>
		/// The password of the Microsoft Account to authenticate.
		/// </summary>
		[JsonProperty("identity_password")]
		public string IdentityPassword { get; set; }

		/// <summary>
		/// If the Microsoft Account has two factor authentication, put the code from the Authenticator app here.
		/// </summary>
		[JsonProperty("identity_two_factor_code")]
		public string IdentityTwoFactorCode { get; set; }
	}
}