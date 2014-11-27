using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XboxLiveApi.Models.Authentication;

namespace XboxLiveApi
{
	public class Authentication
	{
		public static async Task<WindowsLiveAuthenticationResponse> AuthenticateWindowsLiveAsync(string accountIdentity, string accountPassword, string accountSecondFactorAuthenicationCode = null)
		{
			// Call remote api to get tokens
			var httpClient = new HttpClient();
			var response = await httpClient.PostAsync("http://0xdc-xblauthapi.azurewebsites.net/Authentication/Create",
				new StringContent(JsonConvert.SerializeObject(new WindowsLiveAuthentication
				{
					AccountIdentity = accountIdentity,
					AccountPassword = accountPassword,
					AccountSecondFactorAutenticationCode = accountSecondFactorAuthenicationCode
				}), Encoding.UTF8, "application/json"));

			if (!response.IsSuccessStatusCode)
				throw new Exception("Invalid HTTP Status Code");

			var stringReponse = await response.Content.ReadAsStringAsync();
			var parsedReponse = JsonConvert.DeserializeObject<Response<WindowsLiveAuthenticationResponse>>(stringReponse);

			if (parsedReponse.Error != null)
				throw new Exception(parsedReponse.Error.ErrorDescription);

			return parsedReponse.Result;
		}

		public static async Task<bool> AuthenticateXboxLiveAsync(string accessToken)
		{
			// Call xbox live api to get tokens
			var httpClient = new HttpClient();
			var response = await httpClient.PostAsync("http://0xdc-xblauthapi.azurewebsites.net/Authentication/Create",
				new StringContent(JsonConvert.SerializeObject(new XboxLiveAuthenticate
				{
					TokenType = "JWT",
					RelyingParty = "http://auth.xboxlive.com",
					Properties = new XboxLiveAuthenticateProperties
					{
						AuthMethod = "RPS",
						SiteName = "user.auth.xboxlive.com",
						RpsTicket = "t=" + accessToken
					}
				}), Encoding.UTF8, "application/json"));



			return false;
		}
	}
}