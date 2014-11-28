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
			Write("[Debug] Enter Command: ");
			args = new[] { ReadLine() };

			var config = new Configuration();
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
					await Login();

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

		public async Task Login()
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

			var windowsLiveAuthentication = await Authentication.AuthenticateWindowsLiveAsync(identity, identityPassword, identityTwoFactorCode);
			WriteLine("Successful authentication with Windows Live...");

			var xboxLiveAuthentication = await Authentication.AuthenticateXboxLiveAsync(windowsLiveAuthentication.AccessToken);
			WriteLine("Successful authentication with Xbox Live...");

			var xboxLiveAuthorization = await Authentication.AuthorizeXboxLiveAsync(xboxLiveAuthentication.Token);
			WriteLine("Successful authorization with Xbox Live...");

			var settings = GetSettings();
			settings.AccessToken = windowsLiveAuthentication.AccessToken;
			settings.RefreshToken = windowsLiveAuthentication.RefreshToken;
			settings.ExpiresAt = DateTime.UtcNow.AddSeconds(windowsLiveAuthentication.ExpiresIn - 40);
			settings.XboxUserId = xboxLiveAuthorization.DisplayClaims.Xui[0].XboxUserId;
			settings.Token = xboxLiveAuthorization.Token;
			settings.AgeGate = xboxLiveAuthorization.DisplayClaims.Xui[0].AgeGate;
			settings.Gamertag = xboxLiveAuthorization.DisplayClaims.Xui[0].Gamertag;
			settings.UserHeaderSession = xboxLiveAuthorization.DisplayClaims.Xui[0].Uhs;
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
				// do it
				var response = await Authentication.RefreshWindowsLiveAuthenicationAsync(settings.RefreshToken);
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
