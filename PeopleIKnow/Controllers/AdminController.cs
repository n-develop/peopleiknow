using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleIKnow.Models;

namespace PeopleIKnow.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var identityUsers = await _userManager.Users.ToListAsync();
            var users = new List<User>();
            foreach (var identityUser in identityUsers)
            {
                users.Add(new User
                {
                    Id = identityUser.Id,
                    Email = identityUser.Email,
                    IsAdmin = await _userManager.IsInRoleAsync(identityUser, "admin"),
                    IsUser = await _userManager.IsInRoleAsync(identityUser, "user")
                });
            }

            return View(users);
        }

        [HttpPut("Admin/Toggle/{roleName}/{userId}")]
        public async Task<IActionResult> Toggle(string roleName, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            if (await _userManager.IsInRoleAsync(user, roleName))
            {
                var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    return Ok("REVOKED");
                }
            }
            else
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    return Ok("GRANTED");
                }
            }

            return BadRequest();
        }
    }
}