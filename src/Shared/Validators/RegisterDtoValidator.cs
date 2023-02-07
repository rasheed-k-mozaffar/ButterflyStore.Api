using System;
using FluentValidation;

namespace ButterflyStore.Shared.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterUserDto>
	{
		public RegisterDtoValidator()
		{
			//Property => Property.Name
			RuleFor(p => p.FirstName)
				.NotNull().WithMessage("The first name field is required")
				.MaximumLength(25).WithMessage("First name should be less than 25 characters")
				.MinimumLength(2).WithMessage("First name should be greater than 2 characters");

            RuleFor(p => p.LastName)
                .NotNull().WithMessage("The last name field is required")
                .MaximumLength(25).WithMessage("Last name should be less than 25 characters")
                .MinimumLength(2).WithMessage("Last name should be greater than 2 characters");

			RuleFor(p => p.Email)
				.NotNull().WithMessage("The email field is required")
				.EmailAddress().WithMessage("Please enter a valid email address");

            RuleFor(p => p.Password)
                .NotNull().WithMessage("The password field is required")
                .MaximumLength(25).WithMessage("The password should be less than 25 characters")
                .MinimumLength(8).WithMessage("The password should be at least 8 characters");

			RuleFor(p => p.PasswordConfirmation)
				.NotNull().WithMessage("The password confirmation field is required")
				.Equal(p => p.Password).WithMessage("Password and Password Confirmation don't match");
        }
	}
}

