using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using TopLearn.Core.Service.Interface;
using TopLearnDataLayer.Entities.Order;

namespace TopLearn.Web.Pages.Admin.Discount
{
    [Authorize]
    public class CreateDiscountModel : PageModel
    {
        IOrderService _orderService;
        public CreateDiscountModel(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [BindProperty]
        public TopLearnDataLayer.Entities.Order.Discount Discounts {  get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPost(string Startdatepicker, string Enddatepicker)
        {
            if (Startdatepicker != "")
            {
                string[] sd = Startdatepicker.Split('/');
                Discounts.StartDate = new DateTime(int.Parse(sd[0]),
                    int.Parse(sd[1]),
                    int.Parse(sd[2]),
                    new PersianCalendar());
            }

            if (Enddatepicker != "")
            {
                string[] ed = Enddatepicker.Split('/');
                Discounts.EndDate = new DateTime(int.Parse(ed[0]),
                    int.Parse(ed[1]),
                    int.Parse(ed[2]),
                    new PersianCalendar());
            }
            if(!ModelState.IsValid&&_orderService.IsExistCode(Discounts.DiscountCode))
            {
                return Page();
            }
            _orderService.AddDiscount(Discounts);
            return RedirectToPage("index");
        }


        public IActionResult OnGetCheckCode(string code)
        {
            return Content (_orderService.IsExistCode(code).ToString());
        }
    }
}
