using System.Runtime.Serialization;

namespace XboxLiveAuthApi.Models
{
	[DataContract]
	public class Response
	{
		[DataMember(Name = "result")]
		public Result Result { get; set; }

		[DataMember(Name = "error")]
		public Error Error {get;set;}
	}

	[DataContract]
	public class Error
	{
		[DataMember(Name = "error_description")]
		public string ErrorDescription { get; set; }
	}

	[DataContract]
	public abstract class Result {  }
}