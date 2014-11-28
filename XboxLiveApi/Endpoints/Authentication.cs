using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XboxLiveApi.Exceptions;
using XboxLiveApi.Models.Authentication;

namespace XboxLiveApi.Endpoints
{
	public class Authentication
	{
		/// <summary>
		/// Authenticates a Microsoft Account Identity with Windows Live
		/// </summary>
		/// <param name="identity">The email address of the Microsoft Account</param>
		/// <param name="identityPassword">The password of the Microsoft Account</param>
		/// <param name="identityTwoFactorCode">If the Microsoft Account has two factor authentication, put the code from the authenticator app here.</param>
		public static async Task<WindowsLiveAuthenticateResponse> AuthenticateWindowsLiveAsync(string identity, string identityPassword, string identityTwoFactorCode = null)
		{
			// Call remote api to get tokens
			var httpClient = new HttpClient();
			var response = await httpClient.PostAsync("http://xdc-wl-auth-api.herokuapp.com/windowslive/authentication/create",
				new StringContent(JsonConvert.SerializeObject(new WindowsLiveRequest
				{
					Identity = identity,
					IdentityPassword = identityPassword,
					IdentityTwoFactorCode = identityTwoFactorCode
				}), Encoding.UTF8, "application/json"));

			if (!response.IsSuccessStatusCode)
				throw new FriendlyException("Invalid HTTP response from the Windows Live Authentication Server");

			var stringResponse = await response.Content.ReadAsStringAsync();
			WindowsLiveResponse<WindowsLiveAuthenticateResponse> parsedResponse = null;
			try
			{
				parsedResponse = JsonConvert.DeserializeObject<WindowsLiveResponse<WindowsLiveAuthenticateResponse>>(stringResponse);
			}
			catch (Exception)
			{
				throw new FriendlyException("Invalid response from Windows Live Authentication Server");
			}

			if (parsedResponse.Error != null)
				throw new FriendlyException(parsedResponse.Error.ErrorDescription);

			if (parsedResponse.Result == null || parsedResponse.Result.AccessToken == null || parsedResponse.Result.RefreshToken == null)
				throw new FriendlyException("Invalid response from Windows Live Authentication Server");

			return parsedResponse.Result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="refreshToken"></param>
		/// <returns></returns>
		public static async Task<WindowsLiveAuthenticateResponse> RefreshWindowsLiveAuthenicationAsync(string refreshToken)
		{
			var httpClient = new HttpClient();
			var response = await httpClient.PostAsync("https://login.live.com/oauth20_token.srf",
				new StringContent(
					string.Format("client_id=0000000048093EE3&redirect_uri=https://login.live.com/oauth20_desktop.srf&grant_type=refresh_token&scope=service::user.auth.xboxlive.com::MBI_SSL&refresh_token=1", refreshToken), 
					Encoding.UTF8, "application/x-www-form-urlencoded")
				);

			var stringResponse = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{

			}


			//if (!response.IsSuccessStatusCode)
			//	throw new FriendlyException("Invalid HTTP response from the Windows Live Authentication Server");


			return null;
		}

		/// <summary>
		/// Authenticates a Windows Live Access Token with Xbox Live
		/// </summary>
		/// <param name="accessToken">The Windows Live Authentication Token</param>
		public static async Task<XboxLiveResponse<XboxLiveAuthenticateResponse>> AuthenticateXboxLiveAsync(string accessToken)
		{
			// Call xbox live api to get tokens
			var httpClient = new HttpClient();
			var response = await httpClient.PostAsync("https://user.auth.xboxlive.com/user/authenticate",
				new StringContent(JsonConvert.SerializeObject(new XboxLiveRequest<XboxLiveAuthenticate>
				{
					TokenType = "JWT",
					RelyingParty = "http://auth.xboxlive.com",
					Properties = new XboxLiveAuthenticate
					{
						AuthMethod = "RPS",
						SiteName = "user.auth.xboxlive.com",
						RpsTicket = "t=" + accessToken
					}
				}), Encoding.UTF8, "application/json"));

			if (!response.IsSuccessStatusCode)
				throw new FriendlyException("Invalid Windows Live Access Token");

			var stringResponse = await response.Content.ReadAsStringAsync();
			XboxLiveResponse<XboxLiveAuthenticateResponse> parsedResponse = null;
			try
			{
				parsedResponse = JsonConvert.DeserializeObject<XboxLiveResponse<XboxLiveAuthenticateResponse>>(stringResponse);
			}
			catch (Exception)
			{
				throw new FriendlyException("Invalid response from Xbox Live Authentication Server");
			}

			if (parsedResponse.Token == null)
				throw new FriendlyException("Invalid response from Xbox Live Authentication Server");

			return parsedResponse;
		}

		/// <summary>
		/// Authorizes an Xbox Live token
		/// </summary>
		/// <param name="token">The Xbox Live Token</param>
		public static async Task<XboxLiveResponse<XboxLiveAuthorizeResponse>> AuthorizeXboxLiveAsync(string token)
		{
			// Call xbox live api to get tokens
			var httpClient = new HttpClient();
			var response = await httpClient.PostAsync("https://xsts.auth.xboxlive.com/xsts/authorize",
				new StringContent(JsonConvert.SerializeObject(new XboxLiveRequest<XboxLiveAuthorize>
				{
					TokenType = "JWT",
					RelyingParty = "http://xboxlive.com",
					Properties = new XboxLiveAuthorize
					{
						SandboxId = "RETAIL",
						UserTokens = new[]
						{
							token
						}
					}
				}), Encoding.UTF8, "application/json"));

			if (!response.IsSuccessStatusCode)
				throw new FriendlyException("Invalid Xbox Live Token");

			var stringResponse = await response.Content.ReadAsStringAsync();
			XboxLiveResponse<XboxLiveAuthorizeResponse> parsedResponse = null;
			try
			{
				parsedResponse = JsonConvert.DeserializeObject<XboxLiveResponse<XboxLiveAuthorizeResponse>>(stringResponse);
			}
			catch (Exception)
			{
				throw new FriendlyException("Invalid response from Xbox Live Authorization Server");
			}

			if (parsedResponse.Token == null)
				throw new FriendlyException("Invalid response from Xbox Live Authorization Server");

			return parsedResponse;
		}
	}
}