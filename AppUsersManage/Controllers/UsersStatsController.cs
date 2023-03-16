using AppUsersManage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppUsersManage.Controllers
{
    public class UsersStatsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersStatsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UsersStats>();
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UsersStats();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.DateLastVisited = user.DateLastLogin;
                thisViewModel.DateRegistered = user.DateRegistered;
                thisViewModel.Status = user.Status;
                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel);
        }
    }
}
