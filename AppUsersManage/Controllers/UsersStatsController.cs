using AppUsersManage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

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
            List<string> selectedItemsById = accounts["userId"].ToList<string>();
            switch (action)
            {
                case "Block":
                    await applyActionOnUsers(selectedItemsById, user => {
                        user.Status = Status.Blocked;
                        return _userManager.UpdateAsync(user);
                    });
                    break;
                case "Unblock":
                    await applyActionOnUsers(selectedItemsById, user => {
                        user.Status = Status.Active;
                        return _userManager.UpdateAsync(user);
                    });
                    break;
                case "Delete":
                    await applyActionOnUsers(selectedItemsById, user => {
                        return _userManager.DeleteAsync(user);
                    });
                    break;
                default:
                    ModelState.AddModelError(string.Empty, $"Action {action} is not supported");
                    break;
            }
            
            return this.RedirectToAction();
        }

        private async Task applyActionOnUsers(List<string> selectedItemsById, Func<ApplicationUser, Task<IdentityResult>> action)
        {
            foreach (var selectedId in selectedItemsById)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(selectedId);
                var actionResult = await action(user);
                translateErrorsToModelState(actionResult);
            }
        }

        private void translateErrorsToModelState(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
