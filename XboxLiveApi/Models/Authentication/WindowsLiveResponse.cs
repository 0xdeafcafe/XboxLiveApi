using Newtonsoft.Json;

namespace XboxLiveApi.Models.Authentication
{
	public class WindowsLiveResponse<T>
		where T : IWindowsLiveResponseResult
	{
		/// <summary>
		/// The result container from the Windows Live Authentication Server
		/// </summary>
		[JsonProperty("result")]
		public T Result { get; set; }

		/// <summary>
		/// The error container from the Windows Live Authentication Server
		/// </summary>
		[JsonProperty("error")]
		public Error Error { get; set; }
	}

	public class Error
	{
		/// <summary>
		/// The description of the error.
		/// </summary>
		[JsonProperty("error_description")]
		public string ErrorDescription { get; set; }
	}

	public interface IWindowsLiveResponseResult { }
}