using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Service.Interface;
using TopLearnDataLayer.Entities.Order;

namespace TopLearn.Web.Pages.Admin.Discount
{
    [Authorize]
    public class IndexModel : PageModel
    {

        private IOrderService _orderService;
        public IndexModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [BindProperty]
        public List<TopLearnDataLayer.Entities.Order.Discount> Discounts { get; set; }
        public void OnGet()
        {
            Discounts = _orderService.GetAllDiscount( );
        }
    }
}
