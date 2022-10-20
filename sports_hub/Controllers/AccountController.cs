using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using sports_hub.Models;
using sports_hub.Models.Entities;
using sports_hub.Models.ViewModels;
using sports_hub.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace sports_hub.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IImageConverter _imageConverter;
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly ApplicationContext _context;
        
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService,
            IStringLocalizer<AccountController> localizer, ApplicationContext context, IImageConverter imageConverter)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _localizer = localizer;
            _imageConverter = imageConverter;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> LoginAsync()
        {
            LoginViewModel model = new LoginViewModel
            {
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    bool correct_password = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (correct_password)
                    {
                        await _signInManager.SignInAsync(user, false);
                        return RedirectToAction("Index", "Home");
                    }
                }
                model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                ModelState.AddModelError("", _localizer["InvalidEmailOrPassword"]);
            }

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogins(string provider)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new User { });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string remoteError = null)
        {
            LoginViewModel loginViewModel = new LoginViewModel
            {
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, _localizer["ExternalProviderError"] + remoteError);

                return View("Login", loginViewModel);
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                ModelState.AddModelError(string.Empty, _localizer["ErrorLoadingInformation"]);

                return View("Login", loginViewModel);
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new()
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                            LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await _userManager.CreateAsync(user);
                    }

                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");

                }
            }

            return View("Error");

        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }

        [HttpGet]
        public async Task<IActionResult> RegisterAsync()
        {
            RegisterViewModel model = new RegisterViewModel
            {
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Image DefaultImg = Image.FromFile("wwwroot\\imagesAdminPage\\user test icon.png");
                byte[] DefaultImgArr;
                using (MemoryStream ms = new MemoryStream())
                {
                    DefaultImg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    DefaultImgArr = ms.ToArray();
                }
                User user = new()
                {
                    Email = model.Email,
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    ProfileImage = DefaultImgArr,
                    Status = UserStatus.Active,
                    Role = "user",
                    RegistrationDate = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code },
                        protocol: HttpContext.Request.Scheme);
                    var language = HttpContext.Request.Cookies[".AspNetCore.Culture"];
                    await _userManager.AddToRoleAsync(user, "user");
                    await _emailService.SendConfirmationEmailAsync(model.Email, callbackUrl, language);

                    return View("RegisterConfirmation", model);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return View("Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
                return View("Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                    return View("ForgotPasswordConfirmation", model);

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code }, protocol: HttpContext.Request.Scheme);
                var language = HttpContext.Request.Cookies[".AspNetCore.Culture"];

                await _emailService.SendEmailPasswordResetAsync(model.Email, callbackUrl, language);

                return View("ForgotPasswordConfirmation", model);
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null, string userId = null)
        {
            if (code == null || userId == null)
                return View("Error");
            else
                return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ViewData["Message"] = _localizer["InvalidLink"];
                return View("Login");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);

            if (result.Succeeded)
            {
                ViewData["Message"] = _localizer["PasswordUpdated"];
                return View("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.UserInfo = user;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserInfoViewModel model, IFormFile File)
        {


            byte[] imageData = null;

            using (var binaryReader = new BinaryReader(File.OpenReadStream()))
            {
                imageData = binaryReader.ReadBytes((int)File.Length);
            }


            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewData["Message"] = "User not found, please login";
                return View("Login");
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.ProfileImage = imageData;

            
            var result = await _userManager.UpdateAsync(user);
            
            ViewBag.UserInfo = user;
            return View();  
        }

        [HttpGet]
        public IActionResult Profile()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            ViewBag.AvailableLanguages = _context.Languages.ToList();
            ViewBag.urlForShareButtons = Constants.ShareButtonsURL;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordViewModel model)
        {
            ViewBag.AvailableLanguages = _context.Languages.ToList();
            ViewBag.urlForShareButtons = Constants.ShareButtonsURL;
            ViewBag.NewPasswordErrorMessageDisplay = "none";
            ViewBag.MessageDisplay = "none";

            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                return View();
            }

            if (user.PasswordHash == null)
            {
                ViewBag.ThirdPartyErrorMessage = "You can't change password if logged in with third-party service.";
                ViewBag.NewPasswordErrorMessageDisplay = "block";

                return View();
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                ViewBag.NewPasswordErrorMessageDisplay = "none";

                ViewBag.MessageContent = "Your password is successfully updated.";
                ViewBag.MessageTitle = "Updated!";
                ViewBag.MessageDisplay = "block";

                return View();
            }

            foreach (var error in result.Errors)
            {
                if (error.Description == "Incorrect password.")
                    ModelState.AddModelError(string.Empty, "Password does not match.");
                else
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

        public IActionResult TeamHubViewTeams()
        {
            ViewBag.Username = _userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result.UserName;
            return View();
        }

        public IActionResult TeamHubManageTeams()
        {
            ViewBag.Username = _userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result.UserName;
            return View();
        }
    }
}
