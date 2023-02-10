using System;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ButterflyStore.Server.Controllers
{
	public class UserManagementController : BaseController
	{
		//Creating private fields to use after assiging them the values received through the constructor.
		private readonly AppDbContext _context;
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public UserManagementController(AppDbContext context , UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) 
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		/// <summary>
		/// This endpoint returns all the roles.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult GetAllRoles()
		{
			var roles = _roleManager.Roles.ToList();

			return Ok(roles);
		}

		/// <summary>
		/// This method takes a string in the query , and uses it to create a new role
		/// </summary>
		/// <param name="roleName"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> CreateRole(string roleName)
		{
			//Check if the received role name is null or empty
			if(string.IsNullOrEmpty(roleName))
			{
				return BadRequest("Empty Name"); // Return 400 BAD REQUEST STATUS CODE.
			}

			//Check if the role exists or not.
			var existingRole = await _roleManager.RoleExistsAsync(roleName);

			//If the role already exists , return bad request
			if(existingRole)
			{
				return BadRequest(new { Message = "The role already exists" }); //Return 400 BAD REQUEST STATUS CODE.

            }
			else
			{
				//In case it doesn't , create it.
				var roleResult = await _roleManager.CreateAsync(new IdentityRole { Name = roleName });

				if(roleResult.Succeeded)
				{
					return Ok(); //Return 200 OK STATUS CODE.
                }

				//Return a bad request with an anonymous object that takes a list of error descrptions with it.
				return BadRequest(new { errors = roleResult.Errors.Select(e => e.Description) }); //Return 400 BAD REQUEST STATUS CODE.

            }
		}

		/// <summary>
		/// This method returns all the registered users inside our database.
		/// </summary>
		/// <returns></returns>
		[HttpGet("GetUsers")]
		public async Task<IActionResult> GetAllUsers()
		{
			var users = await _userManager.Users.ToListAsync();

			return Ok(users); // Return 200 OK STATUS CODE.
		}


		[HttpPost("AddUserToRole")]
		public async Task<IActionResult> AddUserToRole(string email, string roleName)
		{
			//Check if any of the received data is NULL or EMPTY.
			if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(roleName))
			{
				return BadRequest(); //Return 400 BAD REQUEST STATUS CODE.
            }

			//Check if there's a user that exists with the given email.
			var existingUser = await _userManager.FindByEmailAsync(email);

			if(existingUser == null)
			{
                return BadRequest(new { Message = "No user exists with the given email" }); //Return 400 BAD REQUEST STATUS CODE.
            }

			//Check if there's a role that corresponds to the given role name
			var existingRole = await _roleManager.RoleExistsAsync(roleName);

			if(!existingRole)
			{
				return BadRequest(new {Message = "No role exists with the given name"}); //Return 400 BAD REQUEST STATUS CODE.
            }

			//Add the user retrieved earlier to the received role.
			var result = await _userManager.AddToRoleAsync(existingUser, roleName);

			//Check if the process of adding has succeeded.
			if(result.Succeeded)
			{
				return Ok(); // Return 200 OK STATUS CODE.
			}

			return BadRequest(new { Error = "Something went wrong." });
        }
	}
}

