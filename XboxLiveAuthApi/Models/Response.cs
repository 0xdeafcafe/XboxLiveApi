namespace XboxLiveAuthApi.Models
{
	public class Response
	{
		public Error Error {get;set;}

		public Result Result { get; set; }
	}

	public class Error
	{
		public string ErrorDescription { get; set; }
	}

	public abstract class Result {  }
}