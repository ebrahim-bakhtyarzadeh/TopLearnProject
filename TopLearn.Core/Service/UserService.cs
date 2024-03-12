using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.Convertors;
using TopLearn.Core.Generator;
using TopLearn.Core.Security;
using TopLearn.Core.Service.Interface;
using TopLearnDataLayer.Context;
using TopLearn.DataLayer.Entities;

using TopLearn.DataLayer.Entities.Wallet;
using Microsoft.EntityFrameworkCore;
using TopLearn.Core.DTOs.Users;
using TopLearn.Core.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.Service
{
    public class UserService : IUserService
    {
        TopLearnContext _context { get; set; }
        public UserService(TopLearnContext context)
        {
            _context = context;
        }

        public bool ISExistEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }
        public bool ISExistLogin(LoginViewModel login)
        {
            string password = PasswordHelper.EncodePasswordMd5(login.Password);
            return _context.Users.Any(u => u.Password == password && u.Email == login.Email );
             
        }
        public bool IsExistUserName(string userName)
        {
            return _context.Users.Any(x=> x.UserName == userName);
        }

        public int AddUser(User user)
        {
            _context.Users.Add(user);
             _context.SaveChanges();
            return user.UserId;
            
        }
        public User FindUser(LoginViewModel login)
        {
            string password = PasswordHelper.EncodePasswordMd5(login.Password);
            User user = _context.Users.FirstOrDefault(x=> x.Email == login.Email && x.Password ==password);
            return user;
        }
        public bool ActiveAccount(string ActiveAccount)
        {
            var user = _context.Users.SingleOrDefault(x=>x.ActiveCode == ActiveAccount);
            if(user == null || user.IsActive)
               return false;
            user.IsActive = true;
            user.ActiveCode = NameGenerator.GenerateUniqCode();
            _context.SaveChanges();
            return true;
           
        }

        public User GetUserByUserName(string userName)
        {

            return _context.Users.SingleOrDefault(x => x.UserName == userName);

        }
        public User GetUserByUserId(int UserId)
        {
            return _context.Users.Find(UserId);
        }
        public int GetUserIdByUserName(string username)
        {
            return _context.Users.Single(x => x.UserName == username).UserId;
        }
        public UserPanelViewModel GetUserPanelInformation(string UserName)
        {
            var User = GetUserByUserName(UserName);

            UserPanelViewModel userPanelViewModel = new UserPanelViewModel()
            {
                UserName = User.UserName,
                Email = User.Email,
                RegisterTime = User.RegisterDate,
                Wallet = BalanceUserWallet(UserName)
            };
            return userPanelViewModel;
        }

        public SideBarUserPanelViewModel GetSideBarUserPanelData(string UserName)
        {
            return _context.Users.Where(x => x.UserName == UserName).Select(u=>new SideBarUserPanelViewModel()
            {
                UserName = u.UserName,
                RegisterDate= u.RegisterDate,
                ImageProfilePath =u.UserAvatar 
            }).Single();
        }

        public EditProfileViewModel GetDataForEditProfileUser(string UserName)
        {
            return _context.Users.Where(x => x.UserName == UserName).Select(u => new EditProfileViewModel()
            {
                UserName = u.UserName,
                Email = u.Email,
                AvatarName = u.UserAvatar
            }).Single();
        }
        public void UpdateUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }

        public void EditProfile(string UserName , EditProfileViewModel profile)
        {
            string imagePath = "";
            if (profile.AvatarName != "DefaultAvatar1.jpg")
            {
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", profile.AvatarName);
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }
            profile.AvatarName = NameGenerator.GenerateUniqCode()+Path.GetExtension(profile.UserAvatar.FileName);
            imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Avatars", profile.AvatarName);
            using(var stream = new FileStream(imagePath, FileMode.Create))
            {
                profile.UserAvatar.CopyTo(stream);
            }
            var user = GetUserByUserName(UserName);
            user.UserName = profile.UserName;
            user.Email = profile.Email;
            
            user.UserAvatar = profile.AvatarName;
            UpdateUser(user);
        }

        public bool CompareOldPassword(string OldPassword, string username)
        {
            string hashOldPassword = PasswordHelper.EncodePasswordMd5(OldPassword);
            return _context.Users.Any(x=> x.UserName == username&& x.Password==hashOldPassword);
        }

        public void ChangeUserPassword(string username, string newPassword)
        {
            var user = GetUserByUserName(username);
            user.Password =PasswordHelper.EncodePasswordMd5(newPassword);
            UpdateUser(user);
        }

        public int BalanceUserWallet(string username)
        {
            int userID = GetUserIdByUserName(username);
            var enter = _context.Wallets.Where(w=>w.UserId ==userID && w.TypeId == 1 &&w.IsPay).Select(w=>w.Amount).ToList();
            var exit = _context.Wallets.Where(w => w.UserId == userID && w.TypeId == 2).Select(w => w.Amount).ToList();
            return enter.Sum() - exit.Sum();
        }

        public List<WalletViewModel> GetWallets(string username)
        {
            int userId = GetUserIdByUserName(username);
            return _context.Wallets.Where(w => w.UserId == userId && w.IsPay).Select(w => new WalletViewModel()
            {
                Amount = w.Amount,
                DateTime = w.CreateDate,
                Description = w.Description,
                Type = w.TypeId
           
            }).ToList();
        }

        public int ChargeWallet(string username, int Amount, string description, bool IsPay=false)
        {
            int userID = GetUserIdByUserName(username);
            Wallet wallet = new Wallet()
            {
                Amount = Amount,
                CreateDate = DateTime.Now,
                Description = description,
                IsPay = IsPay,
                TypeId = 1,
                UserId = userID,
                
            };
            return AddWallet(wallet);
        }

        public int AddWallet(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            _context.SaveChanges();
            return wallet.WalletId;
        }
        public Wallet GetWalletByWalletId(int walletId)
        {
            return  _context.Wallets.Find(walletId);
        }

        public void UpdateWallet(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            _context.SaveChanges();
        }
        public UserForAdminViewModel GetUsers(int pageId = 1, string userName="", string filterEmail = "")
        
        {
            IQueryable<User> result = _context.Users;

            if (!string.IsNullOrEmpty(filterEmail))
            {
                result = result.Where(u => u.Email.Contains(filterEmail));
            }
            if (!string.IsNullOrEmpty(userName))
            {
                result = result.Where(u=>u.UserName.Contains(userName));
            }

            //Show Item In Page
            int take =20;
            int skip = (pageId - 1) * take;
            UserForAdminViewModel list = new UserForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = result.Count() / take;
            list.Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();

            return list;
        }

        public int AddUserFromAdmin(CreateUserViewModel user)
        {
            
            User NewUser = new User();
            NewUser.Password = PasswordHelper.EncodePasswordMd5(user.Password);
            NewUser.ActiveCode = NameGenerator.GenerateUniqCode();
            NewUser.Email = user.Email;
            NewUser.UserName = user.UserName;
           
            NewUser.IsActive = true;
            NewUser.RegisterDate= DateTime.Now;
            #region Save Avatar
            if (user.UserAvatar != null)
            {
                NewUser.UserAvatar = SaveImage(user.UserAvatar);
            }
            else
            {
                NewUser.UserAvatar = "DefaultAvatar1.jpg";
            }
           
            #endregion




            return AddUser(NewUser);
        }

        public EditUserViewModel GetUserForShowInEditMode(int userId)
        {

            return _context.Users.Where(u => u.UserId == userId)
                .Select(u => new EditUserViewModel()
                {
                    UserId = u.UserId,
                    AvatarName = u.UserAvatar,
                    Email = u.Email,
                    UserName = u.UserName,
                    UserRoles = u.UserRoles.Select(r => r.RoleId).ToList()
                }).Single();
        }

        public void EditUserFromAdmin(EditUserViewModel user)
        {
            User SelectedUser = GetUserByUserId(user.UserId);
            SelectedUser.Email = user.Email;
            if(!string.IsNullOrEmpty(user.Password))
            {
                SelectedUser.Password = PasswordHelper.EncodePasswordMd5(user.Password);
            }
            if (user.UserAvatar != null)
            {
                //Delete Old Image
                if(user.AvatarName!= "DefaultAvatar1.jpg")
                {
                    string DeletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Avatars", user.AvatarName);
                    if(File.Exists(DeletePath))
                    {
                        File.Delete(DeletePath);
                    }
                }
               //Save New Image
                SelectedUser.UserAvatar= SaveImage(user.UserAvatar);
                _context.Users.Update(SelectedUser);
                _context.SaveChanges();

            }

        }

        public string SaveImage(IFormFile user)
        {
            
          
                string imagePath = "";

                string UserAvatar = NameGenerator.GenerateUniqCode() + Path.GetExtension(user.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Avatars", UserAvatar);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    user.CopyTo(stream);
                }
            return UserAvatar;
        }

        public UserForAdminViewModel GetDeleteUsers(int pageId = 1, string filterEmail = "", string userName = "")
        {
            IQueryable<User> result = _context.Users.IgnoreQueryFilters().Where(x=>x.IsDelete==true);

            if (!string.IsNullOrEmpty(filterEmail))
            {
                result = result.Where(u => u.Email.Contains(filterEmail));
            }
            if (!string.IsNullOrEmpty(userName))
            {
                result = result.Where(u => u.UserName.Contains(userName));
            }

            //Show Item In Page
            int take = 20;
            int skip = (pageId - 1) * take;
            UserForAdminViewModel list = new UserForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = result.Count() / take;
            list.Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();
            return list;
        }

        public void DeleteUser(int userId)
        {
            User user = GetUserByUserId(userId);
            user.IsDelete = true;
            UpdateUser(user);
        }
    }
}
