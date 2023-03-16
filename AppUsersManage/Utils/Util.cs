using AppUsersManage.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace AppUsersManage.Utils
{
    public static class Util
    {
        public static async Task<bool> SaveLastLoginDateAsync(UserManager<ApplicationUser> _userManager, string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return false;
            }
            user.DateLastLogin = DateTime.UtcNow;
            var lastLoginResult = await _userManager.UpdateAsync(user);
            if (!lastLoginResult.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred setting the last login date" +
                    $" ({lastLoginResult.ToString()}) for user with ID '{user.Id}'.");
            }
            return true;
        }
    }
}
