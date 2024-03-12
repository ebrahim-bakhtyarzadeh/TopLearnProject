using EnvDTE;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopLearn.Core.Service;
using TopLearn.Core.Service.Interface;

namespace TopLearn.Web.Controllers
{
    
    public class HomeController : Controller
    {
        IUserService _userService;
        ICourseService _CourseService;
        IOrderService _orderService;
        public HomeController(IUserService userService , ICourseService CourseService , IOrderService orderService)
        {
            _userService = userService;
            _CourseService = CourseService;
            _orderService = orderService;
        }


        public IActionResult Index()
        {
            ViewBag.PopularCourse= _CourseService.GetPopularCourse();
            return View(_CourseService.GetCourse());
        }


        [Authorize]
        [Route("OnlinePayment/{id}")]
        public IActionResult OnlinePayment(int id)
        {
            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
                HttpContext.Request.Query["Authority"] != "")
            {
                string Authority = HttpContext.Request.Query["Authority"];
                var wallet = _userService.GetWalletByWalletId(id);
                var Payment = new ZarinpalSandbox.Payment(wallet.Amount);
                var res = Payment.Verification(Authority).Result;
                if ( res.Status == 100)
                {
                    ViewBag.code = res.RefId;
                    ViewBag.IsSuccess = true;
                    wallet.IsPay = true;
                    _userService.UpdateWallet(wallet);
                }
            }
            return View();
        }
        public IActionResult GetSubGroups(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem(){Text="انتخاب کنید",Value="" }
            };
            list.AddRange(_CourseService.GetSubGroupForManageCourse(id));
            return Json(new SelectList(list, "Value", "Text"));
        }
        [HttpPost]
        [Route("file-upload")]
        public IActionResult UploadImage(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();



            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/MyImages",
                fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);

            }



            var url = $"{"/MyImages/"}{fileName}";


            return Json(new { uploaded = true, url });
        }
        
        public IActionResult Error404()
        {
            return View();
        }
        
    }
}
