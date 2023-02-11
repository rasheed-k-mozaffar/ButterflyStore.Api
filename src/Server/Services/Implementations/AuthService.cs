using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ButterflyStore.Server.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService (
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
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
                LastName = model.LastName
            };

            //Creating a new cart with the ID of the user.
            user.Cart = new Cart { UserId = user.Id };

            await _context.Carts.AddAsync(user.Cart);
            //Create the new user with the given credentials.
            var result = await _userManager.CreateAsync(user, model.Password!);

            if (result.Succeeded)
            {
                //Adding the newely registered user to the "App User Role".
                await _userManager.AddToRoleAsync(user, "AppUser");

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

        public async Task<ApiAuthResponse> LoginUserAsync(LoginUserDto model)
        {
            if(model == null)
            {
                throw new ArgumentNullException("model is null");
            }

            //Check if a user exists with the same recieved email.
            var existingUser = await _userManager.FindByEmailAsync(model.Email!);

            if(existingUser == null)
            {
                //Tell the user that the given email is not a register email.
                return new ApiAuthResponse
                {
                    Message = "The entered email isn't registered.",
                };
            }
            else
            {
                //Check the password against the given email , if it's valid for that specific email
                //then generate a JWT security token and send it to the user.
                var result = await _userManager.CheckPasswordAsync(existingUser, model.Password!);

                if(result)
                {
                    var token = await GenerateJwtToken(existingUser);
                    return new ApiAuthResponse
                    {
                        Message = token,
                        HasSucceeded = true
                    };
                }
                else
                {
                    //If the check fails , throw an error message for the user.
                    return new ApiAuthResponse
                    {
                        Message = "Something has gone wrong.",
                        HasSucceeded = false
                    };
                }
            }
        }

        /// <summary>
        /// This method takes a user , and generates a JWT token with a set of claims and returns it as a string.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>a JWT token as a string.</returns>
        private async Task<string> GenerateJwtToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]!);

            var claims = await GetAllClaims(user);
            
            //This part describes the structure of the JWT token.
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenAsString = tokenHandler.WriteToken(token);

            return tokenAsString;
        }


        private async Task<List<Claim>> GetAllClaims(AppUser user)
        {
            //Create a list of claims for the user.
            var claims = new List<Claim>
            {
                new Claim("Id" , user.Id),
                new Claim(JwtRegisteredClaimNames.Sub , user.Email!),
                new Claim(JwtRegisteredClaimNames.Email , user.Email!),
                new Claim(JwtRegisteredClaimNames.GivenName , user.FirstName!),
                new Claim(JwtRegisteredClaimNames.FamilyName , user.LastName!),
                new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString())
            };

            //Get all the claims that have been assigned to the user.
            var userClaims = await _userManager.GetClaimsAsync(user);

            //Add those claims to our claims list.
            claims.AddRange(userClaims);

            //Get the user roles.
            //By default , every new user will have the AppUser role.
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach(var userRole in userRoles)
            {
                //retrieving the role
                var role = await _roleManager.FindByNameAsync(userRole);

                //Check if the role exists or not.
                if(role != null)
                {
                    //Adding the role to the list of claims
                    claims.Add(new Claim(ClaimTypes.Role, userRole));

                    //Getting the role claims , which are claims that can provided for a certain role.
                    var roleClaims = await _roleManager.GetClaimsAsync(role);

                    foreach(var roleClaim in roleClaims)
                    {
                        //Lastly , if there are any role claims , add them to the list of claims here.
                        claims.Add(roleClaim);
                    }
                }
            }

            return claims;
        }

    }
}

