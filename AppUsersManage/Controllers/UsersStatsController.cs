using AppUsersManage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppUsersManage.Controllers
{

    [Authorize]
    public class UsersStatsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersStatsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UsersStats>();
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UsersStats();
                thisViewModel.UserId = user.Id;
                thisViewModel.UserName = user.UserName;
                thisViewModel.Email = user.Email;
                thisViewModel.DateLastVisited = user.DateLastLogin;
                thisViewModel.DateRegistered = user.DateRegistered;
                thisViewModel.Status = user.Status;
                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel);
        }

        [HttpPost]
        [ActionName("Index")]
        public async Task<IActionResult> IndexPost(IFormCollection accounts, string action)
        {
            List<string> areSelectedItemsById = accounts["userId"].ToList<string>();
            switch (action)
            {
                case "Block":
                    foreach (var areSelectedId in areSelectedItemsById)
                    {
                        ApplicationUser user = await _userManager.FindByIdAsync(areSelectedId);
                        user.Status = Status.Blocked;
                        var updateResult = await _userManager.UpdateAsync(user);
                        foreach (var error in updateResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }

                    }
                    break;
                case "Unblock":
                    foreach (var areSelectedId in areSelectedItemsById)
                    {
                        ApplicationUser user = await _userManager.FindByIdAsync(areSelectedId);
                        user.Status = Status.Active;
                        var updateResult = await _userManager.UpdateAsync(user);
                        foreach (var error in updateResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                    break;
                case "Delete":
                    foreach (var areSelectedId in areSelectedItemsById)
                    {
                        ApplicationUser user = await _userManager.FindByIdAsync(areSelectedId);
                        var deleteResult = await _userManager.DeleteAsync(user);
                        foreach (var error in deleteResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                    break;
                default:
                    ModelState.AddModelError(string.Empty, $"Action {action} is not supported");
                    break;
            }
            MessageBox.Show("Successfully saved", " Student Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return this.RedirectToAction();
        }

    }
}
