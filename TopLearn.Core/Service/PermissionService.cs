using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.Service.Interface;
using TopLearn.DataLayer.Entities.Permissions;
using TopLearn.DataLayer.Entities.User;
using TopLearnDataLayer.Context;


namespace TopLearn.Core.Service
{
    public class PermissionService : IPermissionService
    {
        private TopLearnContext _context;

        public PermissionService(TopLearnContext context)
        {
            _context = context;
        }

        
        public List<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }

        public void AddRolesToUser(List<int> roleIds, int userId)
        {
            foreach (int roleId in roleIds)
            {
                _context.UserRoles.Add(new UserRole()
                {
                    RoleId = roleId,
                    UserId = userId
                });
            }

            _context.SaveChanges();
        }

        public void EditRolesUser(int userId, List<int> rolesId)
        {
            //Delete All Roles User
            _context.UserRoles.Where(r => r.UserId == userId).ToList().ForEach(r => _context.UserRoles.Remove(r));

            //Add New Roles
            AddRolesToUser(rolesId, userId);
        }

        public int AddRole(Role role)
        {
           _context.Roles.Add(role);
            _context.SaveChanges();
            return role.RoleId;
        }

        public Role GetRoleById(int id)
        {
            return _context.Roles.Find(id);
        }

        public void UpdateRole(Role Role)
        {
            
            _context.Roles.Update(Role);
            _context.SaveChanges();
        }

        public void DeleteRole(Role role)
        {
            
            role.IsDelete = true;
            UpdateRole(role);
        }

        public void EditRole(Role role)
        {
            
            UpdateRole(role);
            
        }
        #region Permission
        public List<Permission> GetAllPermission()
        {
            return _context.Permission.ToList();
        }

        public void AddPermissionToRole(int roleId, List<int> permissions)
        {
            foreach (var p in permissions)
            {
                _context.RolePermission.Add(new RolePermission
                {
                    PermissionId = p,
                    RoleId = roleId

                });
                _context.SaveChanges();
            }
        }

        public List<RolePermission> GetAllPermissionRole()
        {
           return _context.RolePermission.ToList();
        }

        public void UpdatePermissionRole(int roleId, List<int> Permission)
        {
            _context.RolePermission.Where(p=>p.RoleId ==roleId ).ToList().ForEach(p=>_context.RolePermission.Remove(p));
            AddPermissionToRole(roleId, Permission);
        }
        public int GetUserByUsername (string username)
        {
            return _context.Users.Single(u => u.UserName == username).UserId;
        }
        public bool CheckPermission(int permissionId, string username)
        {
            int userId = GetUserByUsername(username);

            List<int> UserRoles =_context.UserRoles.Where(r=>r.UserId == userId).Select(r=>r.RoleId).ToList();

            if(!UserRoles.Any())
                return false;

            List<int> RolesPermossion =  _context.RolePermission.Where( p=>p.PermissionId ==permissionId ).Select(p=>p.RoleId).ToList();
             
            return RolesPermossion.Any( p=> UserRoles.Contains(p));
        }
        #endregion

    }
}
