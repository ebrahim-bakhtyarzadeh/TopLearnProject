using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using TopLearn.Core.Service;
using TopLearn.Core.Service.Interface;
using TopLearn.DataLayer.Entities.User;
using TopLearnDataLayer.Entities.Order;

namespace TopLearn.Web.Pages.Admin.Discount
{
    [Authorize]
    public class EditDiscountModel : PageModel
    {
        private IOrderService _OrderService { get; set; }
        public EditDiscountModel(IOrderService orderService) 
        {
            _OrderService = orderService;
        }
        [BindProperty]
        public TopLearnDataLayer.Entities.Order.Discount Discount { get; set; }

        public void OnGet(int id)
        {
            Discount = _OrderService.GetDiscountById(id);
        }


        public IActionResult OnPost(string Startdatepicker, string Enddatepicker)
        {
            if (Startdatepicker != "")
            {
                string[] sd = Startdatepicker.Split('/');
                Discount.StartDate = new DateTime(int.Parse(sd[0]),
                    int.Parse(sd[1]),
                    int.Parse(sd[2]),
                    new PersianCalendar());
            }

            if (Enddatepicker != "")
            {
                string[] ed = Enddatepicker.Split('/');
                Discount.EndDate = new DateTime(int.Parse(ed[0]),
                    int.Parse(ed[1]),
                    int.Parse(ed[2]),
                    new PersianCalendar());
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _OrderService.UpdateDiscount(Discount);
            return RedirectToPage("index");
        }
    }
}
