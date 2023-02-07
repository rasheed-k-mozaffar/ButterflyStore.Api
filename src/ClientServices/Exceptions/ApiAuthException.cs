using System;
using ButterflyStore.Shared.ApiResponses;

namespace ButterflyStore.Client.Exceptions
{
	public class ApiAuthException : Exception
	{
		public ApiAuthResponse ErrorResponse { get; set; }

		public ApiAuthException(ApiAuthResponse response , string message) : base(message)
		{
			ErrorResponse = response;
		}
	}
}