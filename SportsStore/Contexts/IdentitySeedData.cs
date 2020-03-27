using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace SportsStore.Contexts
{
    /*
     * Hard-coding the details of an administrator account is often required so that you can log into
       an application once it has been deployed and start administering it. When you do this, you must remember to
       change the password for the account you have created. See Chapter 28 for details of how to change passwords
       using Identity.
    */
    public static class IdentitySeedData
    {
        private const string AdminUser = "Admin";
        private const string AdminPassword = "Secret123$";

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            var userManager = app.ApplicationServices.GetRequiredService<UserManager<IdentityUser>>();
            var user = await userManager.FindByIdAsync(AdminUser);
            if (user == null)
            {
                user = new IdentityUser(AdminUser);
                await userManager.CreateAsync(user, AdminPassword);
            }
        }
    }
}