using Microsoft.Framework.ConfigurationModel;

namespace XboxLiveApi.Console
{
	public class Program
	{
		public void Main(string[] args)
		{
			var config = new Configuration()
				.AddJsonFile(".settings")
				.AddEnvironmentVariables();



			System.Console.ReadLine();
		}
	}
}
