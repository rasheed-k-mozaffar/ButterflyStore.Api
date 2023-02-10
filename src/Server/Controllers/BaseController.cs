using Microsoft.AspNetCore.Mvc;
using Treblle.Net.Core;

namespace ButterflyStore.Server.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
//[Treblle]
public class BaseController : ControllerBase
{

}