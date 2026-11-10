using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Models.Account;

namespace TARge25Shop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailServices _emailServices;

        public AccountController
            (
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            IEmailServices emailServices
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailServices = emailServices;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = vm.Email,
                    Name = vm.Name,
                    Email = vm.Email,
                    City = vm.City

                };

                var result = await _userManager.CreateAsync(user, vm.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);

                    EmailTokenDto newsignup = new();
                    newsignup.Token = token;
                    newsignup.Body = $"Please confirm your email by clicking on the link: <a href='{confirmationLink}'>Confirm Email</a>";
                    newsignup.Subject = "Shop confirm your email";
                    newsignup.To = user.Email;

                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }

                    _emailServices.SendEmailToken(newsignup, token);
                    List<string> errordatas =
                        [
                        "Area", "Accounts",
                        "Issue", "Success",
                        "StatusMessage", "Registration Success",
                        "ActedOn", $"{vm.Email}",
                        "CreatedAccountData", $"{vm.Email}\n{vm.City}\n[password hidden]\n[password hidden]"
                        ];
                    ViewBag.ErrorData = errordatas;
                    ViewBag.ErrorTitle = "Registration Success";
                    ViewBag.ErrorMessage = "Please check your email to confirm your account.";

                    return View("ConfirmationEmailMessage");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(vm);
        }
    }
}
