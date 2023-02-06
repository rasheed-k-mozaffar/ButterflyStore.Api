using System;
namespace ButterflyStore.Shared.ApiResponses
{
	public class ApiAuthResponse
	{
		public string? Message { get; set; }
		public bool HasSucceeded { get; set; }
		public List<string>? Errors { get; set; }
	}
}

