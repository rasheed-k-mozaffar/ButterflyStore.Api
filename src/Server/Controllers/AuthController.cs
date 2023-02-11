using ButterflyStore.Shared.Validators;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Treblle.Net.Core;

namespace ButterflyStore.Server.Controllers
{
	public class AuthController : BaseController
	{
		private readonly IAuthService _authService;
		private readonly IValidator<RegisterUserDto> _registerValidator;
		private readonly IValidator<LoginUserDto> _loginValidator;

		public AuthController(IAuthService authService , IValidator<RegisterUserDto> registerValidator , IValidator<LoginUserDto> loginValidator)
		{
			_authService = authService;
			_registerValidator = registerValidator;
			_loginValidator = loginValidator;
		}
 
        [HttpPost("register")]
		public async Task<IActionResult> RegisterUser(RegisterUserDto model)
		{
			ValidationResult validationResult = await _registerValidator.ValidateAsync(model);
			//Check the validations , in case everything is valid , Register the user
			if (validationResult.IsValid)
			{
				var result = await _authService.RegisterUserAsync(model);

				if (result.HasSucceeded)
				{
					return Ok(result); //Return 200 OK STATUS CODE.
				}

				return BadRequest(result);
			}
			else
			{
				validationResult.AddToModelState(ModelState);
				//In case some validations are broken.
				return BadRequest(ModelState); //Return 400 BAD REQUEST STATUS CODE WITH THE VALIDATIONS.
			}
		}

		[HttpPost("login")]
		public async Task<IActionResult> LoginUser(LoginUserDto model)
		{
			ValidationResult validationResult = await _loginValidator.ValidateAsync(model);
			//Check if the recieved DTO is valid or not.
			if(validationResult.IsValid)
			{
				var result = await _authService.LoginUserAsync(model);

				if(result.HasSucceeded)
				{
					return Ok(result); //Return 200 OK STATUS CODE.
				}

				return BadRequest(result); //Return 400 BAD REQUEST STATUS CODE WITH THE ERROR MESSAGE.
			}

			validationResult.AddToModelState(ModelState);

			return BadRequest(ModelState); //Return 400 BAD REQUEST STATUS CODE WITH MODEL VALIDATIONS.
        }
    }

	

	public static class Extensions
	{
		public static void AddToModelState(this ValidationResult result , ModelStateDictionary modelState)
		{
			foreach (var error in result.Errors)
			{
				modelState.AddModelError(error.PropertyName, error.ErrorMessage);
			}
		}
	}
}

