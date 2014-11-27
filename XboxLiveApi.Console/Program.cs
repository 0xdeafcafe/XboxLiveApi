using Microsoft.Framework.ConfigurationModel;
using System.Threading.Tasks;

namespace XboxLiveApi.Console
{
	public class Program
	{
		public void Main(string[] args)
		{
			var config = new Configuration();
			config.AddJsonFile(".settings");
			config.AddEnvironmentVariables();

			Run(config).Wait();
			System.Console.ReadLine();
		}

		public async Task Run(Configuration config)
		{
			
		}

	}
}
