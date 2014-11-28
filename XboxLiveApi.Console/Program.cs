using Microsoft.Framework.ConfigurationModel;
using System.Threading.Tasks;
using System.Console;
using XboxLiveApi.Console.Models;
using System.IO;
using Newtonsoft.Json;
using XboxLiveApi.Endpoints;
using System;

namespace XboxLiveApi.Console
{
	public class Program
	{
		private const string SettingsPath = ".settings";

		public void Main(string[] args)
		{
			Task t = MainAsync(args);
			t.Wait();
		}

		public async Task MainAsync(string[] args)
		{
			// TODO: remove this
#if DEBUG
			if (args == null || args.Length == 0)
			{
				Write("[Debug] Enter Command: ");
				args = new[] { ReadLine() };
			}
#endif

			var config = new Configuration();
			config.AddJsonFile("config.json");
			config.AddEnvironmentVariables();

			// check args
			if (args == null ||
				args.Length == 0 ||
				args[0].ToLowerInvariant() == "h" ||
				args[0].ToLowerInvariant() == "help")
			{
				// write help
				var commands =
					Environment.NewLine + Environment.NewLine +
					"Xbox Live Enviroment Api - Build " + "alpha 0001" +
					Environment.NewLine + Environment.NewLine +
					"USAGE: xbl <command> [options]" +
					Environment.NewLine + Environment.NewLine +

					"  xbl login				description_1" + Environment.NewLine +
					"  xbl refresh				description_2" + Environment.NewLine +
					"  xbl help					description_3" + Environment.NewLine;

				WriteLine(commands);

#if DEBUG
				ReadLine();
#endif

				return;
			}

			var validInput = false;
			switch (args[0].ToLowerInvariant())
			{
				case "login":
					string windowsLiveAuthenticationServer = null;
					if (!config.TryGet("windows_live_authentication_server", out windowsLiveAuthenticationServer))
						windowsLiveAuthenticationServer = "http://xdc-wl-auth-api.herokuapp.com/windowslive/authentication/create";
					await Login(windowsLiveAuthenticationServer);

					validInput = true;
					break;

				case "refresh":
					if (!ValidateAuth()) return;
					await RefreshTokens(true);

					validInput = true;
					break;

				case "summary":
					if (!ValidateAuth()) return;
					await RefreshTokens();
					var settings = GetSettings();

					await Xuid.Test(settings.Token, settings.UserHeaderSession);

					validInput = true;
					break;
			}

			if (!validInput)
			{
				WriteLine("No valid command specified - type `xbl help` for a list of options");
			}

#if DEBUG
			ReadLine();
#endif
		}

		public async Task Login(string windowsLiveAuthServer)
		{
			// get identity
			Write("Microsoft Account Email Address: ");
			var identity = ReadLine();

			// get password
			Write("Microsoft Account Password: ");
			string identityPassword = null;
			ConsoleKeyInfo key;
			do
			{
				key = ReadKey(true);
				if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
				{
					identityPassword += key.KeyChar;
					Write("*");
				}
				else
				{
					if (key.Key == ConsoleKey.Backspace && identityPassword.Length > 0)
					{
						identityPassword = identityPassword.Substring(0, (identityPassword.Length - 1));
						Write("\b \b");
					}
				}
			}
			while (key.Key != ConsoleKey.Enter);
			WriteLine();

			// get two fac
			Write("Microsoft Account Two Factor Code (Optional): ");
			var identityTwoFactorCode = ReadLine();

			Write("Authenticating with Windows Live... ");
			var windowsLiveAuthentication = await Authentication.AuthenticateWindowsLiveAsync(windowsLiveAuthServer, identity, identityPassword, identityTwoFactorCode);
			WriteLine("Done");

			Write("Authenticating with Xbox Live... ");
			var xboxLiveAuthentication = await Authentication.AuthenticateXboxLiveAsync(windowsLiveAuthentication.AccessToken);
			WriteLine("Done");

			Write("Authorizing with Xbox Live... ");
			var xboxLiveAuthorization = await Authentication.AuthorizeXboxLiveAsync(xboxLiveAuthentication.Token);
			WriteLine("Done");

			var settings = GetSettings();
			settings.AccessToken = windowsLiveAuthentication.AccessToken;
			settings.RefreshToken = windowsLiveAuthentication.RefreshToken;
			settings.ExpiresAt = DateTime.UtcNow.AddSeconds(windowsLiveAuthentication.ExpiresIn - 40);
			settings.XboxUserId = xboxLiveAuthorization.DisplayClaims.Xui[0].XboxUserId;
			settings.Token = xboxLiveAuthorization.Token;
			settings.AgeGate = xboxLiveAuthorization.DisplayClaims.Xui[0].AgeGate;
			settings.Gamertag = xboxLiveAuthorization.DisplayClaims.Xui[0].Gamertag;
			settings.UserHeaderSession = xboxLiveAuthorization.DisplayClaims.Xui[0].UserHeaderSession;
			SetSettings(settings);

			WriteLine();
			WriteLine(" --- ");
			WriteLine();
			WriteLine("Signed in as {0} ({1}) - {2} account.", settings.Gamertag, settings.XboxUserId, settings.AgeGate);
		}

		public bool ValidateAuth()
		{
			var settings = GetSettings();
			if (!settings.SettingsAreValid())
			{
				WriteLine("You are not authenticated. Execute the `xbl login` command");
				return false;
			}

			return true;
		}

		public async Task RefreshTokens(bool force = false)
		{
			var settings = GetSettings();
			if (settings.ExpiresAt < DateTime.UtcNow || force)
			{
				Write("Refreshing Windows Live Access Token... ");
				var windowsLiveRefresh = await Authentication.RefreshWindowsLiveAuthenicationAsync(settings.RefreshToken);
				WriteLine("Done");

				WriteLine("Updating Authentication with Xbox Live... ");
				var xboxLiveAuthentication = await Authentication.AuthenticateXboxLiveAsync(windowsLiveRefresh.AccessToken);
				WriteLine("Done");

				WriteLine("Updating Authorization with Xbox Live... ");
				var xboxLiveAuthorization = await Authentication.AuthorizeXboxLiveAsync(xboxLiveAuthentication.Token);
				WriteLine("Done");

				settings.AccessToken = windowsLiveRefresh.AccessToken;
				settings.AgeGate = xboxLiveAuthorization.DisplayClaims.Xui[0].AgeGate;
				settings.ExpiresAt = DateTime.UtcNow.AddSeconds(windowsLiveRefresh.ExpiresIn - 20);
				settings.Gamertag = xboxLiveAuthorization.DisplayClaims.Xui[0].Gamertag;
				settings.RefreshToken = windowsLiveRefresh.RefreshToken;
				settings.Token = xboxLiveAuthorization.Token;
				settings.UserHeaderSession = xboxLiveAuthorization.DisplayClaims.Xui[0].UserHeaderSession;
				settings.XboxUserId = xboxLiveAuthorization.DisplayClaims.Xui[0].XboxUserId;

				SetSettings(settings);
			}
		}

		public Settings GetSettings()
		{
			if (!File.Exists(SettingsPath))
				return new Settings();

			var str = File.ReadAllText(SettingsPath);
			Settings settings = null;
			try
			{
				settings = JsonConvert.DeserializeObject<Settings>(str);
			}
			catch
			{
				return new Settings();
			}
			return settings ?? new Settings();
		}

		public void SetSettings(Settings settings)
		{
			File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(settings, Formatting.Indented));
		}
	}
}
