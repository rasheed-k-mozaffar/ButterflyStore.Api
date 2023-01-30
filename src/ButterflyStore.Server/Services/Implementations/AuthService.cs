using System;
using Microsoft.AspNetCore.Identity;

namespace ButterflyStore.Server.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<AppUser> userManager , IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }


        public async Task<ApiAuthResponse> RegisterUserAsync(RegisterUserDto model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model is null");
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email!);

            //Check if a user already exists with the same email , if so , return an error message.
            if (existingUser != null)
            {
                return new ApiAuthResponse { Message = "Email is already in use." };
            }

            AppUser user = new AppUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            //Creating a new cart with the ID of the user.
            user.Cart = new Cart { UserId = user.Id };

            //Create the new user with the given credentials.
            var result = await _userManager.CreateAsync(user, model.Password!);

            if (result.Succeeded)
            {
                //In case everything went well.
                return new ApiAuthResponse
                {
                    Message = "Your account was created successfully",
                    HasSucceeded = true,
                    Errors = null
                };
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description).ToList();

                //In case something went wrong.
                return new ApiAuthResponse
                {
                    Message = "Something went wrong",
                    HasSucceeded = false,
                    Errors = errors
                };
            }
        }

        public Task<ApiAuthResponse> LoginUserAsync(LoginUserDto model)
        {
            throw new NotImplementedException();
        }

    }
}

