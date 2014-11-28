namespace XboxLiveApi.Models.Authentication
{
	public class XboxLiveRequest<T>
		where T : IXboxLiveRequestProperties
	{
		/// <summary>
		/// The type of token you're submitting - generally `JWT`
		/// </summary>
		public string TokenType { get; set; }

		/// <summary>
		/// The website of the party you're requesting data from
		/// </summary>
		public string RelyingParty { get; set; }

		/// <summary>
		/// Properties of the Xbox Live Request
		/// </summary>
		public T Properties { get; set; }
	}

	public interface IXboxLiveRequestProperties { }
}