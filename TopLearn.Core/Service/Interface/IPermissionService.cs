using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.Permissions;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.Service.Interface
{
    public interface IPermissionService
    {
        #region Roles
        List<Role> GetRoles();
        Role GetRoleById(int id);
        void AddRolesToUser(List<int> roleIds,int userId);
        void EditRolesUser(int  userId,List<int> rolesId);
        int AddRole(Role role);
        void UpdateRole(Role Role);
        void DeleteRole(Role role);
        void EditRole (Role role);
        #endregion

        #region permission

        public List<Permission> GetAllPermission();
        void AddPermissionToRole(int roleId, List<int> permissions);
        List<RolePermission> GetAllPermissionRole();
        void UpdatePermissionRole(int roleId, List<int> Permission);
        bool CheckPermission(int permissionId, string username);
        #endregion
    }
}
