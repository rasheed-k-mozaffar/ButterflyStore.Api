using Microsoft.AspNetCore.Mvc;

namespace ButterflyStore.Server.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class BaseController : ControllerBase
{

}