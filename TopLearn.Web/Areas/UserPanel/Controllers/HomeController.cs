using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TopLearn.Core.DTOs;
using TopLearn.Core.DTOs.Users;
using TopLearn.Core.Service.Interface;

namespace TopLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class HomeController : Controller
    {
        private IUserService _userService;
        public HomeController(IUserService userService)
        {
            _userService = userService;
        }
        
        public IActionResult Index()
        {
            return View(_userService.GetUserPanelInformation(User.Identity.Name));
        }
        [Route("UserPanel/EditProfile")]
        public IActionResult EditProfile()
        {

            return View(_userService.GetDataForEditProfileUser(User.Identity.Name));
        }
        [Route("UserPanel/EditProfile")]
        [HttpPost]
        public IActionResult EditProfile(EditProfileViewModel profile)
        {
            if(!ModelState.IsValid)
                return View(profile);
            _userService.EditProfile(User.Identity.Name, profile);

            return Redirect("/Logout");

        }
        [Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword() 
        {
            return View();
        }
        [Route("UserPanel/ChangePassword")]
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel changePassword)
        {
            string CurrentUserName = User.Identity.Name;
            if(!ModelState.IsValid)
                return View(changePassword);
            if(!_userService.CompareOldPassword(changePassword.OldPassword, CurrentUserName))
            {
                ModelState.AddModelError("OldPassworg", "کلمه عبور فعلی صحیح نمیباشد");
                return View(changePassword);
            }

            _userService.ChangeUserPassword(CurrentUserName, changePassword.Password);
            ViewBag.IsSuccess = true; 
            return View();
        }


    }
}
