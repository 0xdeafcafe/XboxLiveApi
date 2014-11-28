using System.Net.Http;
using System.Threading.Tasks;

namespace XboxLiveApi.Endpoints
{
	public class Xuid
	{
		/// <summary>
		/// Authorizes an Xbox Live token
		/// </summary>
		/// <param name="token">The Xbox Live Token</param>
		public static async Task Test(string token, string Uhs)
		{
			// Call xbox live api to get tokens
			var httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", string.Format("XBL3.0 x={0};{1}", Uhs, token));
			var response = await httpClient.GetAsync("https://social.xboxlive.com/users/xuid(2533274956338602)/summary");

			var stringResponse = await response.Content.ReadAsStringAsync();

			var x = 1;
		}
	}
}