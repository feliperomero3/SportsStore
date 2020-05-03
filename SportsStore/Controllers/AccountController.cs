using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userMgr,
            SignInManager<IdentityUser> signInMgr)
        {
            _userManager = userMgr;
            _signInManager = signInMgr;
        }

        [AllowAnonymous]
        public ViewResult Login(string returnUrl)
        {
            return View(new LoginModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginModel.Name);

                if (user != null)
                {
                    await _signInManager.SignOutAsync();

                    var result = await _signInManager.PasswordSignInAsync(user,
                        loginModel.Password, false, false);

                    if (result.Succeeded)
                    {
                        return Redirect(loginModel.ReturnUrl ?? "/Admin/Index");
                    }
                }
            }

            ModelState.AddModelError("", "Invalid name or password");

            return View(loginModel);
        }

        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}