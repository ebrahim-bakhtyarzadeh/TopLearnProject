using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.DTOs;
using TopLearn.Core.DTOs.Users;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;


namespace TopLearn.Core.Service.Interface
{
    public interface IUserService
    {

        bool IsExistUserName(string userName);
        bool ISExistEmail(string email);
        int AddUser(User user);
        User GetUserByUserName(string userName);
        User GetUserByUserId(int UserId);
        User FindUser(LoginViewModel login);
        public bool ISExistLogin(LoginViewModel login);
        bool ActiveAccount(string ActiveCode);
        int GetUserIdByUserName(string username);
            
        #region UserPanel
        UserPanelViewModel GetUserPanelInformation(string UserName);
        SideBarUserPanelViewModel GetSideBarUserPanelData(string UserName);
        EditProfileViewModel GetDataForEditProfileUser(string UserName);
        void EditProfile(string UserName ,EditProfileViewModel profile);
        public void UpdateUser(User user);
        public bool CompareOldPassword(string Password,string username);
        public void ChangeUserPassword(string username , string newPassword);
        #endregion

        #region Wallet
        public int BalanceUserWallet(string username);
        List<WalletViewModel> GetWallets(string username);
        int ChargeWallet(string username, int Amount, string description,bool IsPay =false);
        int AddWallet(Wallet wallet);
        Wallet GetWalletByWalletId(int walletId);
        void UpdateWallet(Wallet wallet);
        #endregion

        #region AdminPanel
        UserForAdminViewModel GetUsers(int pageId = 1, string filterEmail = "", string userName = "");

        int AddUserFromAdmin(CreateUserViewModel user);
        public EditUserViewModel GetUserForShowInEditMode(int userId);
        void EditUserFromAdmin(EditUserViewModel user);
        UserForAdminViewModel GetDeleteUsers(int pageId = 1, string filterEmail = "", string userName = "");
        public void DeleteUser(int userId);
        #endregion












        #region Commands used
        string SaveImage(IFormFile user);
        #endregion
    }
}
