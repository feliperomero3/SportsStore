using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SportsStore.Contexts
{
    /* Caution:
     * Hard-coding the details of an administrator account is often required so that you can log into
       an application once it has been deployed and start administering it. When you do this, you must remember to
       change the password for the account you have created. See Chapter 28 for details of how to change passwords
       using Identity.
    */
    public static class IdentitySeedData
    {
        private const string AdminUser = "Admin";
        private const string AdminPassword = "Secret123$";

        public static async Task SeedAsync(UserManager<IdentityUser> userManager)
        {
            var user = await userManager.FindByIdAsync(AdminUser);
            if (user == null)
            {
                user = new IdentityUser(AdminUser);
                await userManager.CreateAsync(user, AdminPassword);
            }
        }
    }
}