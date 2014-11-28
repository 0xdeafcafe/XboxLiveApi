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
		/// <param name="userHeaderSession">The unique UserHeaderSession used for authentication</param>
		public static async Task Summary(string token, string userHeaderSession, string xuid)
		{
			// Call xbox live api to get tokens
			var httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", string.Format("XBL3.0 x={0};{1}", userHeaderSession, token));
			var response = await httpClient.GetAsync("https://social.xboxlive.com/users/xuid(" + xuid + ")/summary");

			var stringResponse = await response.Content.ReadAsStringAsync();

			var x = 1;
		}
	}
}