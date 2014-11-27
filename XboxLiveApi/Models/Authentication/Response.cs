using Newtonsoft.Json;

namespace XboxLiveApi.Models.Authentication
{
	public class Response<T>
		where T : Result
	{
		[JsonProperty("result")]
		public T Result { get; set; }

		[JsonProperty("result")]
		public Error Error { get; set; }
	}

	public class Error
	{
		[JsonProperty("error_description")]
		public string ErrorDescription { get; set; }
	}

	public abstract class Result { }
}