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
            List<string> areSelectedItemsById = GetSelectedItemsByIdFromForm(accounts);
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

                        /*var lockUser = await _userManager.SetLockoutEnabledAsync(user, true);
                        var endDate = new DateTime(2222, 06, 06);
                        var lockDate = await _userManager.SetLockoutEndDateAsync(user, endDate);

                        if (!lockUser.Succeeded || !lockDate.Succeeded)
                        {
                            ModelState.AddModelError(string.Empty, $"Can't block user with email {user.Email}");
                        }*/
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
                        /*var lockUser = await _userManager.SetLockoutEnabledAsync(user, false);
                        var lockDate = await _userManager.SetLockoutEndDateAsync(user, DateTime.Now - TimeSpan.FromMinutes(1));
                        if (!lockUser.Succeeded || !lockDate.Succeeded)
                        {
                            ModelState.AddModelError(string.Empty, $"Can't block user with email {user.Email}");
                        }*/
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
            
            return this.RedirectToAction();
        }

        private List<string> GetSelectedItemsByIdFromForm(IFormCollection accounts)
        {
            var selectedItemsById = new List<string>();
            IEnumerable<string> Ids = accounts.Keys.Where(key => key.StartsWith("id_"));
            int lengthOfPrefix = 3;//length of starting "id_"
            foreach (string key in Ids)
            {
                string id = key.Substring(3);
                bool isChecked = accounts[key].Count > 1;
                if (isChecked)
                {
                    selectedItemsById.Add(id);
                }
            }
            return selectedItemsById;
        }

    }
}
