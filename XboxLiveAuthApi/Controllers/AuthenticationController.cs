using OpenQA.Selenium.PhantomJS;
using System;
using System.Web.Http;
using XboxLiveAuthApi.Models;

namespace XboxLiveAuthApi.Controllers
{
	public class AuthenticationController : ApiController
	{
		private const string _liveAuthorizeUri = "https://login.live.com/oauth20_authorize.srf?client_id={0}&redirect_uri={1}&response_type={2}&display={3}&scope={4}";
		private const string _clientId = "0000000048093EE3";
		private const string _redirectUri = "https://login.live.com/oauth20_desktop.srf";
		private const string _responseType = "token";
		private const string _display = "touch";
		private const string _scope = "service::user.auth.xboxlive.com::MBI_SSL";


		// POST: /Authentication/Create
		[HttpPost]
		public Response Create([FromBody] Identity identity)
		{
			using (var browser = new PhantomJSDriver())
			{
				try
				{
					browser.Navigate().GoToUrl(string.Format(_liveAuthorizeUri, _clientId, _redirectUri, _responseType, _display, _scope));
					browser.FindElementById("i0116").SendKeys(identity.AccountIdentity);
					browser.FindElementById("i0118").SendKeys(identity.AccountPassword);
					browser.FindElementByName("SI").Click();

					if (string.Equals(browser.Title, "Help us protect your account", StringComparison.InvariantCultureIgnoreCase))
					{
						if (string.IsNullOrWhiteSpace(identity.AccountSecondFactorAutenticationCode))
							return new Response { Error = new Error { ErrorDescription = "This account has Two Factor authentication. Please enter the authentication code from the authorization app" } };

						// 2factor auth
						browser.FindElementByName("otc").SendKeys(identity.AccountSecondFactorAutenticationCode);
						browser.FindElementById("idSubmit_SAOTCC_Continue").Click();
					}

					// Pull AccessToken out of url
					var start = browser.Url.IndexOf('#') + 1;
					var urlData = browser.Url.Remove(0, start);
					var datas = urlData.Split('&');

					var accessToken = "";
					var refreshToken = "";
					var tokenType = "";
					int expiresIn = 0;
					string scope = "";
					long userId = 0;

					foreach (var data in datas)
					{
						var parts = data.Split('=');
						switch (parts[0])
						{
							case "access_token":
								accessToken = parts[1];
								break;
							case "refresh_token":
								refreshToken = parts[1];
								break;
							case "token_type":
								tokenType = parts[1];
								break;
							case "expires_in":
								expiresIn = int.Parse(parts[1]);
								break;
							case "scope":
								scope = parts[1];
								break;
							case "user_id":
								userId = long.Parse(parts[1], System.Globalization.NumberStyles.HexNumber);
								break;
						}
					}

					return new Response
					{
						Result = new Authentication
						{
							AccessToken = accessToken,
							RefreshToken = refreshToken,
							TokenType = tokenType,
							ExpiresIn = expiresIn,
							ExpiresAt = DateTime.UtcNow.AddSeconds(expiresIn - 2),
							Scope = scope,
							UserId = userId
						}
					};
				}
				finally
				{
					browser.Close();
				}
			}
		}
	}
}