using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs;
using TopLearn.Core.Security;
using TopLearn.Core.Service.Interface;

namespace TopLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(6)]
    public class DeleteUserModel : PageModel
    {
        IUserService _userService;
        
        public DeleteUserModel(IUserService userService)
        {
            _userService = userService;
        }
        public UserForAdminViewModel UserForAdminViewModel { get; set; }
        public void OnGet(int pageId = 1, string filterUserName = "", string filterEmail = "")
        {
            UserForAdminViewModel = _userService.GetDeleteUsers(pageId, filterUserName, filterEmail);
        }
    }
}
