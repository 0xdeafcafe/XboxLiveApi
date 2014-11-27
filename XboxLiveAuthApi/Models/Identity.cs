using System;
namespace XboxLiveAuthApi.Models
{
	public class Identity
	{
		public string AccountIdentity { get; set; }

		public string AccountPassword { get; set; }

		public string AccountSecondFactorAutenticationCode { get; set; }
	}
}