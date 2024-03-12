using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs;
using TopLearn.Core.Security;
using TopLearn.Core.Service.Interface;

namespace TopLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(7)]
    public class EditUserModel : PageModel
    {
        private IUserService _userService;
        private IPermissionService _permissionService;

        public EditUserModel(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }
        [BindProperty]
        public EditUserViewModel EditUserViewModel { get; set; }
       
        public void OnGet(int id)
        {
            ViewData["IsSuccess"] = true;
            EditUserViewModel = _userService.GetUserForShowInEditMode(id);
            ViewData["Roles"] = _permissionService.GetRoles();
         
        }
        public IActionResult OnPost(List<int> SelectedRoles)
        {
           if(EditUserViewModel.Email ==null ||EditUserViewModel.Password==null || EditUserViewModel.AvatarName == null)
            {
                EditUserViewModel = _userService.GetUserForShowInEditMode(EditUserViewModel.UserId);
                ViewData["Roles"] = _permissionService.GetRoles();
                ViewData["IsSuccess"] = false;
                return Page();
            }

            _userService.EditUserFromAdmin(EditUserViewModel);

            //Edit Roles
            _permissionService.EditRolesUser(EditUserViewModel.UserId, SelectedRoles);
            return RedirectToPage("Index");
        }
    }
}
