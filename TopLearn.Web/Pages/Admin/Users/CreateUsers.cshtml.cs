using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs;
using TopLearn.Core.Security;
using TopLearn.Core.Service.Interface;

namespace TopLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(8)]
    public class CreateUsersModel : PageModel
    {
        private IPermissionService _permissionService;
        private IUserService _userService;
        public CreateUsersModel(IPermissionService permissionService , IUserService userService)
        {
            _permissionService = permissionService;
            _userService = userService; 
        }
        [BindProperty]
        public CreateUserViewModel CreateUserViewModel { get; set; }
        public void OnGet()
        {
            ViewData["Roles"] = _permissionService.GetRoles();
        }


        public IActionResult OnPost(List<int> SelectedRoles=null)
        {
            if (CreateUserViewModel.UserName == null || CreateUserViewModel.Password==null || CreateUserViewModel.Email==null)
            {
                ViewData["Roles"] = _permissionService.GetRoles();
                return Page();
            }

               
            int userId = _userService.AddUserFromAdmin(CreateUserViewModel);

            //Add Roles
            _permissionService.AddRolesToUser(SelectedRoles, userId);

            return Redirect("/Admin/Users");
        }
    }
}
