using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TopLearn.Core.Convertors;
using TopLearn.Core.DTOs.Users;
using TopLearn.Core.Generator;
using TopLearn.Core.Security;
using TopLearn.Core.Senders;
using TopLearn.Core.Service;
using TopLearn.Core.Service.Interface;
using TopLearn.DataLayer.Entities.User;


namespace TopLearn.Web.Controllers
{
    public class AccountController : Controller
    {
        private IUserService _userService;
        private IViewRenderService _viewRenderService;
        public AccountController(IUserService user, IViewRenderService viewRenderService)
        {
            _userService = user;
            _viewRenderService = viewRenderService;
        }

        #region Register
        [Route("Register")]
       
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }
            if (_userService.IsExistUserName(register.UserName))
            {
                ModelState.AddModelError("UserName", "نام کاربری معتبر نمی باشد");
                return View(register);
            }
            //if (_userService.ISExistEmail(register.Email))
            //{
            //    ModelState.AddModelError(register.Email.FixEmail(), "ایمیل وارد شده معتبر نیست");
            //    return View(register);
            //}

            User User = new User()
            {
                ActiveCode = NameGenerator.GenerateUniqCode(),
                Email = FixedText.FixEmail(register.Email),
                IsActive = false,
                Password = PasswordHelper.EncodePasswordMd5(register.Password),
                RegisterDate = DateTime.Now,
                UserAvatar = "DefaultAvatar1.jpg",
                UserName = register.UserName
            };
            _userService.AddUser(User);

            #region Send Activation Email
            string Body = _viewRenderService.RenderToStringAsync("ViewSendEmail", User);
            SendEmail.Send(User.Email, "فعالسازی", Body);
            #endregion

            return View("SuccessRegister", User);
        }
        #endregion

        #region Login
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }
        [Route("Login")]
        [HttpPost]
        public IActionResult Login(LoginViewModel loginView , string ReturnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                return View(loginView);
            }
            if (!_userService.ISExistLogin(loginView))
            {
                ModelState.AddModelError("Email", "کاربری یافت نشد . لطفا در وارد کردن ایمیل و رمز عبور دقت کنید");
                return View(loginView);
            }
            else
            {
                User user = _userService.FindUser(loginView);
                if (user.IsActive)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                        new Claim(ClaimTypes.Name , user.UserName)
                    };
                    var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                    var pricipal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties()
                    {
                        IsPersistent = loginView.ReMemberMe
                    };
                     HttpContext.SignInAsync(pricipal, properties);

                    ViewBag.IsSuccess = true;
                    if(ReturnUrl != "/")
                    {
                        return Redirect(ReturnUrl);
                    }
                    return View();
                }
                else
                {
                    ModelState.AddModelError("Email", "ایمیل شما فعال نمی باشد");
                    return View(loginView);
                }


            }


        }
        #endregion

        #region Logout
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
        }
        #endregion

        #region Active Account
        public IActionResult ActiveAccount(string id)
        {
            ViewBag.IsActive = _userService.ActiveAccount(id);
            return View();
        }
        #endregion
    }
}
