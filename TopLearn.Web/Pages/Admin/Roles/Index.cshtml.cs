using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs;
using TopLearn.Core.DTOs.Users;
using TopLearn.Core.Security;
using TopLearn.Core.Service.Interface;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Web.Pages.Admin.Roles
{
    [PermissionChecker(1)]
    public class IndexModel : PageModel
    {
        IPermissionService _permissionService;
        public IndexModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }


        public List<Role> RolesList { get; set; }
        public void OnGet()
        {
            
            RolesList = _permissionService.GetRoles();
            ViewData["Permission"] = _permissionService.GetAllPermission();
            ViewData["SelectedPermission"] = _permissionService.GetAllPermissionRole();
        }
        public IActionResult OnPost(string AddRoleTitle, int DeleteRoleId, int EditRoleId ,string EditTitle ,string TitleDelete,List<int> SelectedPermission)
        {
            if(AddRoleTitle != null && SelectedPermission!=null)
            {
                int roleId = _permissionService.AddRole(new Role()
                {
                    IsDelete = false,
                    RoleTitle = AddRoleTitle
                });
                _permissionService.AddPermissionToRole(roleId, SelectedPermission);
                
            }
            if (DeleteRoleId != 0)
            {
                _permissionService.DeleteRole(new Role()
                {
                    
                    RoleId = DeleteRoleId,
                    RoleTitle = TitleDelete
                });
               
            }
            if(EditTitle != null && EditRoleId != 0) 
            {
                _permissionService.EditRole(new Role()
                {
                    RoleId = EditRoleId,
                    RoleTitle = EditTitle
                });
                _permissionService.UpdatePermissionRole(EditRoleId, SelectedPermission);
            }


            return RedirectToPage("Index");

        }
         
    }
}
