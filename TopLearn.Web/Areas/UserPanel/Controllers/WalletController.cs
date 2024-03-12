using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.DTOs.Users;
using TopLearn.Core.Service.Interface;

namespace TopLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class WalletController : Controller
    {
        public IUserService _UserService { get; set; }
        public WalletController(IUserService userService)
        {
            _UserService = userService;
        }


        [Route("UserPanel/Wallet")]
        public IActionResult Index()
        {
            ViewBag.ListWallet = _UserService.GetWallets(User.Identity.Name);

            return View();
        }
        [Route("UserPanel/Wallet")]
        [HttpPost]
        public IActionResult Index(ChargeWalletViewModel charge)
        {   
            if(!ModelState.IsValid)
            {
                ViewBag.ListWallet = _UserService.GetWallets(User.Identity.Name);
                return View(charge);
            }
            int walletID = _UserService.ChargeWallet(User.Identity.Name, charge.Amount,"شارژ حساب");
            ViewBag.ListWallet = _UserService.GetWallets(User.Identity.Name);
            #region Online payment


            var payment = new ZarinpalSandbox.Payment(charge.Amount);

            var Res = payment.PaymentRequest("شارژ کیف پول", "https://localhost:44392/OnlinePayment/" + walletID, "Programming92020@gmail.com");
            if (Res.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + Res.Result.Authority);
            }
          
            #endregion


            return null; ;
        }
    }
}
