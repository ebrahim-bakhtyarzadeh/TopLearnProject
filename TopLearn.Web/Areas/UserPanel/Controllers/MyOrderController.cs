using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.DTOs.Orders;
using TopLearn.Core.Service.Interface;

namespace TopLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class MyOrderController : Controller
    {
        public readonly IOrderService _OrderService;

        public MyOrderController(IOrderService orderService)
        {
            _OrderService = orderService;
        }
        public IActionResult Index()
        {
           return View(_OrderService.GetUserOrders(User.Identity.Name));
        }

        [Route("/UserPanel/MyOrder/ShowOrder/{OrderId}")]
        public IActionResult ShowOrder(int OrderId ,bool finaly=false,string type="")
        {

            var order = _OrderService.GetOrderForUserPanel(User.Identity.Name, OrderId);

            if(order == null)
            {
                return NotFound();
            }

            ViewBag.finaly=finaly;
            ViewBag.typeDisCount = type;
            return View(order);
        }
        
        public IActionResult FinalyOrder(int OrderId)
        {
            if (_OrderService.FinalyOrder(User.Identity.Name, OrderId))
            {
                return Redirect("/UserPanel/MyOrder/ShowOrder/" + OrderId + "?finaly=true");
            }
            return BadRequest();
        }

        public IActionResult UseDiscount(int OrderId,string code)
        {
            DiscountUseType type = _OrderService.UseDiscount(OrderId, code);
            return Redirect("/UserPanel/MyOrder/ShowOrder/"+ OrderId + "?type=" + type.ToString());
        }
    }
}
