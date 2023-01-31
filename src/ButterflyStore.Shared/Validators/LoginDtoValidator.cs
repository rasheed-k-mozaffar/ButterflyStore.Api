using FluentValidation;

namespace ButterflyStore.Shared.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginUserDto>
	{
		public LoginDtoValidator()
		{
            //Property => Property.Name
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("The email field is required")
                .EmailAddress().WithMessage("Please enter a valid email address");

            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("The password field is required")
                .MaximumLength(25).WithMessage("The password should be less than 25 characters")
                .MinimumLength(8).WithMessage("The password should be at least 8 characters");
        }
	}
}

