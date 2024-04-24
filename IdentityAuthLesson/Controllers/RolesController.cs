using IdentityAuthLesson.DTOs;
using IdentityAuthLesson.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityAuthLesson.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDTO>> CreateRole(RoleDTO role)
        {
            var result = await _roleManager.FindByNameAsync(role.RoleName);

            if (result == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(role.RoleName));

                return Ok(new ResponseDTO
                {
                    Message = "Role Succesfully Created!",
                    IsSuccess = true,
                    StatusCode = 201
                });
            }

            return Ok(new ResponseDTO
            {
                Message = "Client Error The Role is not created!",
                StatusCode = 403
            });
        }

        [HttpGet]
        public async Task<ActionResult<List<IdentityRole>>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return Ok(roles);
        }

        [HttpPut]
        public async Task<ActionResult<IdentityRole>> UpdateRole(Guid id, RoleDTO newRole)
        {
            var currentRole = await _roleManager.FindByIdAsync(id.ToString());

            if (currentRole != null)
            {
                currentRole.Name = newRole.RoleName;

                try
                {
                    var result = await _roleManager.UpdateAsync(currentRole);
                    if (result.Succeeded)
                    {
                        return Ok(currentRole);
                    }
                    else
                    {
                        return BadRequest("Failed to update.");
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            else
            {
                return NotFound($"Role with ID {id} not found.");
            }
        }

        [HttpDelete]
        public async Task<ActionResult<string>> DeleteRole(Guid id)
        {
            var Role = await _roleManager.FindByIdAsync(id.ToString());
            if (Role != null)
            {
                var result = await _roleManager.DeleteAsync(Role);
                return Ok("Moshniy Deleted");
            }
            return Ok("Role not found");
        }
    }
}
