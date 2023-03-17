using AppUsersManage.Models;
using Microsoft.AspNetCore.Identity;

namespace AppUsersManage.UserDestroyerMiddleware
{
    public class UserDestroyerMiddleware
    {
        private readonly RequestDelegate _next;

        public UserDestroyerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            if (!string.IsNullOrEmpty(httpContext.User.Identity.Name))
            {
                var user = await userManager.FindByNameAsync(httpContext.User.Identity.Name);

                if (user?.Status != Status.Active)
                {
                    //Log the user out and redirect back to homepage
                    await signInManager.SignOutAsync();
                    httpContext.Response.Redirect("/Identity/Account/Login");
                }
            }
            await _next(httpContext);
        }
    }

    public static class UserDestroyerMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserDestroyer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserDestroyerMiddleware>();
        }
    }
}
