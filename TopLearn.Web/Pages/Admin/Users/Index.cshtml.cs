using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs;
using TopLearn.Core.DTOs.Users;
using TopLearn.Core.Security;
using TopLearn.Core.Service.Interface;

namespace TopLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(5)]
    public class IndexModel : PageModel
    {
        IUserService _userService;
        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }
        public UserForAdminViewModel UserForAdminViewModel { get; set; }
        public void OnGet(int pageId =1 , string filterUserName = "" , string filterEmail = "")
        {
            UserForAdminViewModel = _userService.GetUsers(pageId , filterUserName , filterEmail);
        }
        public IActionResult OnPost(int UserId)
        {
            _userService.DeleteUser(UserId);
            return RedirectToPage("Index");
        }
         
    }
}
