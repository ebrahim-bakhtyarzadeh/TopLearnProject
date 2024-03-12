using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.DTOs.Orders;
using TopLearnDataLayer.Entities.Order;

namespace TopLearn.Core.Service.Interface
{
    public interface IOrderService
    {
        public int AddOrder(string username, int courseId);
        public void UpdatePriceOrder(int OrderId);
        Order GetOrderForUserPanel(string username, int orderid);
        bool FinalyOrder(string username, int orderid);
        List<Order> GetUserOrders(string username);
        Order GetOrderById(int orderId);
        void UpdateOrder(Order order);
        bool IsUserInCourse (string username,int courseId);

        #region Discount
        DiscountUseType UseDiscount(int orderId, string code);
        void AddDiscount(Discount discount);
        public List<Discount> GetAllDiscount();
        Discount GetDiscountById(int DiscountId);
        void UpdateDiscount(Discount discount);
        bool IsExistCode(string code);
        #endregion
    }
}
