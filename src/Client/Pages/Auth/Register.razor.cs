using System;
using Microsoft.AspNetCore.Components;


namespace ButterflyStore.Client.Pages.Auth
{
	public partial class Register : ComponentBase
	{
		[Inject]
		private HttpClient HttpClient { get; set; }




		public Register()
		{
		}
	}
}

