using System;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ButterflyStore.Server.Controllers
{
	[Authorize(Roles = "AppUser")]
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


		//ROLE RELATED ENDPOINTS.


		/// <summary>
		/// This endpoint returns all the roles.
		/// </summary>
		/// <returns></returns>
		[HttpGet("GetRoles")]
		public IActionResult GetAllRoles()
		{
			var roles = _roleManager.Roles.ToList();

			return Ok(roles);
		}

		/// <summary>
		/// This endpoint takes a string in the query , and uses it to create a new role
		/// </summary>
		/// <param name="roleName"></param>
		/// <returns></returns>
		[HttpPost("CreateRole")]
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
		/// This endpoint deletes a role based on its ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("DeleteRole/{id}")]
		public async Task<IActionResult> DeleteRole(string id)
		{
			//Check if the ID is null or empty
			if(string.IsNullOrEmpty(id))
			{
				return BadRequest("The ID is invalid"); //Return 400 BAD REQUEST STATUS CODE.
            }

			//Retrieve the role from the database using the given ID
			var roleToRemove = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);

			//Check if a role with that ID exists or not
			if(roleToRemove != null)
			{
				//In case a role exists , then remove it
				_context.Roles.Remove(roleToRemove);

				await _context.SaveChangesAsync();

				return NoContent(); //Return 204 NO CONTENT STATUS CODE.
			}
			else
			{
				//In case no role was find that corresponds to that ID , then return an error message.
				return BadRequest("No role was found with the given ID"); //Return 400 BAD REQUEST STATUS CODE.
            }

		}


		//USER RELATED ENDPOINTS.


		/// <summary>
		/// This endpoint returns all the registered users inside our database.
		/// </summary>
		/// <returns></returns>
		[HttpGet("GetUsers")]
		public async Task<IActionResult> GetAllUsers()
		{
			var users = await _userManager.Users.ToListAsync();

			return Ok(users); // Return 200 OK STATUS CODE.
		}


		/// <summary>
		/// This endpoint deletes a user based on its ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("DeleteUser/{id}")]
		public async Task<IActionResult> DeleteUser(string id)
		{
            //Check if the ID is null or empty
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("The ID is invalid"); //Return 400 BAD REQUEST STATUS CODE.
            }

			var userToDelete = await _userManager.FindByIdAsync(id);

			//Check if user exists with the given ID or not.
			if(userToDelete != null)
			{
				_context.Users.Remove(userToDelete);

				await _context.SaveChangesAsync();

				return NoContent(); //Return 204 NO CONTENT STATUS CODE.
            }
			else
			{
				return BadRequest("No user was found with the given ID"); //Return 400 BAD REQUEST STATUS CODE.
            }
        }


		/// <summary>
		/// This endpoint gets a user using the given email , if the user exists , it adds to the given role
		/// Also , we check if the role exists or not , in case it doesn't , we return a bad request.
		/// </summary>
		/// <param name="email"></param>
		/// <param name="roleName"></param>
		/// <returns></returns>
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


		/// <summary>
		/// This endpoint returns all the roles given to a certain user
		/// </summary>
		/// <param name="email">We use the email to fetch the user from the database./param>
		/// <returns></returns>
		[HttpGet("GetUserRoles")]
		public async Task<IActionResult> GetUserRoles(string email)
		{
            //Check if any of the received email is NULL or EMPTY.
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { Message = "Please enter a valid email."}); //Return 400 BAD REQUEST STATUS CODE.
            }

			var user = await _userManager.FindByEmailAsync(email);

			//Check if a user exists with that email or not
			if(user == null)
			{
				return BadRequest(new { Message = "No user was found with the given email." }); //Return 400 BAD REQUEST STATUS CODE.
            }

			//Retrieve the roles for that user.
			var userRoles = await _userManager.GetRolesAsync(user);

			return Ok(userRoles); // Return 200 OK STATUS CODE.
        }


        /// <summary>
        /// This endpoint removes a user from a specifc role ,  , and the role name t
        /// </summary>
        /// <param name="email">We use the email to get the user</param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpPost("RemoveUserFromRole")]
		public async Task<IActionResult> RemoveUserFromRole(string email , string roleName)
		{
            //Check if any of the received data is NULL or EMPTY.
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(roleName))
            {
                return BadRequest(); //Return 400 BAD REQUEST STATUS CODE.
            }


			//Check if user exists.
			var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BadRequest(new { Message = "No user was found with the given email." }); //Return 400 BAD REQUEST STATUS CODE.
            }

			//Check if role exists.

			var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                return BadRequest(new { Message = "No role exists with the given name" }); //Return 400 BAD REQUEST STATUS CODE.
            }

			var result = await _userManager.RemoveFromRoleAsync(user, roleName);

			if(result.Succeeded)
			{
                return Ok(); // Return 200 OK STATUS CODE.
            }
			else
			{
				return BadRequest(new { Message = "Something went wrong while attempting to remove the user from the role." }); //Return 400 BAD REQUEST STATUS CODE.
            }
        }

	}
}

