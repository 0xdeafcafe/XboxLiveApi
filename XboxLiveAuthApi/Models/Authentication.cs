using System;
using System.Runtime.Serialization;

namespace XboxLiveAuthApi.Models
{
	[DataContract]
	public class Authentication 
		: Result
	{
		[DataMember(Name = "access_token")]
		public string AccessToken { get; set; }

		[DataMember(Name = "refresh_token")]
		public string RefreshToken { get; set; }

		[DataMember(Name = "token_type")]
		public string TokenType { get; set; }

		[DataMember(Name = "expires_in")]
		public int ExpiresIn { get; set; }

		[DataMember(Name = "expires_at")]
		public DateTime ExpiresAt { get; set; }

		[DataMember(Name = "scope")]
		public string Scope { get; set; }

		[DataMember(Name = "user_id")]
		public long UserId { get; set; }
	}
}