using EnvDTE;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.DTOs.Orders;
using TopLearn.Core.Service.Interface;
using TopLearn.DataLayer.Entities.Course;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;
using TopLearnDataLayer.Context;
using TopLearnDataLayer.Entities.Order;

namespace TopLearn.Core.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUserService _UserService;
        public readonly TopLearnContext _Context;

        public OrderService(IUserService UserService, TopLearnContext context)
        {
            _UserService = UserService;
            _Context = context;
        }
        public int AddOrder(string username, int courseId)
        {
            int userId = _UserService.GetUserIdByUserName(username);

            Order order = _Context.Orders
                .FirstOrDefault(o => o.UserId == userId && !o.IsFinaly);


            var Course = _Context.Courses.Find(courseId);

            if (order == null)
            {
                order = new Order()
                {
                    UserId = userId,
                    IsFinaly = false,
                    CreateDate = DateTime.Now,
                    OrderSum = Course.CoursePrice,
                    OrderDetails = new List<OrderDetail>()
                    {
                        new OrderDetail()
                        {
                            CourseId =courseId,
                            Count = 1,

                            Price = Course.CoursePrice


                        }
                    }
                };
                _Context.Orders.Add(order);
                _Context.SaveChanges();
            }
            else
            {
                OrderDetail detail = _Context.OrderDetails
                    .FirstOrDefault(d => d.OrderId == order.OrderId && d.CourseId == courseId);

                if (detail != null)
                {
                    detail.Count += 1;
                    _Context.OrderDetails.Update(detail);
                }
                else
                {
                    detail = new OrderDetail()
                    {
                        OrderId = order.OrderId,
                        Count = 1,
                        CourseId = courseId,
                        Price = Course.CoursePrice
                    };
                    _Context.OrderDetails.Add(detail);

                }
                _Context.SaveChanges();
                UpdatePriceOrder(order.OrderId);
            }


            return order.OrderId;
        }

        public void UpdatePriceOrder(int OrderId)
        {
            var order = _Context.Orders.Find(OrderId);
            order.OrderSum = _Context.OrderDetails.Where(d => d.OrderId == OrderId).Sum(d => d.Price);
            _Context.Orders.Update(order);
            _Context.SaveChanges();
        }

        public Order GetOrderForUserPanel(string username, int orderid)
        {
            int userId = _UserService.GetUserIdByUserName(username);
            return _Context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Course)
               .FirstOrDefault(o => o.UserId == userId && o.OrderId == orderid);
        }
        public Order GetOrderById(int orderId)
        {
            return _Context.Orders.Find(orderId);
        }

        public void UpdateOrder(Order order)
        {
            _Context.Orders.Update(order);
            _Context.SaveChanges();
        }

        public bool FinalyOrder(string username, int orderid)
        {
            int userId = _UserService.GetUserIdByUserName(username);
            var Order = _Context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Course)
                .FirstOrDefault(o => o.OrderId == orderid && o.UserId == userId);

            if (Order == null || Order.IsFinaly)
            {
                return false;
            }
            if (_UserService.BalanceUserWallet(username) >= Order.OrderSum)
            {
                Order.IsFinaly = true;
                _UserService.AddWallet(new Wallet()
                {
                    Amount = Order.OrderSum,
                    CreateDate = DateTime.Now,
                    IsPay = true,
                    Description = "فاکتور شما #" + Order.OrderId,
                    UserId = userId,
                    TypeId = 2
                });
                _Context.Orders.Update(Order);

                foreach (var detail in Order.OrderDetails)
                {
                    _Context.UserCourses.Add(new UserCourse()
                    {
                        CourseId = detail.CourseId,
                        UserId = userId,
                    });
                }
                _Context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Order> GetUserOrders(string username)
        {
            int userId = _UserService.GetUserIdByUserName(username);
            return _Context.Orders.Where(o => o.UserId == userId).ToList();
        }

        public DiscountUseType UseDiscount(int orderId, string code)
        {
            var Discount = _Context.Discounts.SingleOrDefault(d => d.DiscountCode == code);
            if (Discount == null)
            {
                return DiscountUseType.NotFound;
            }
            if (Discount.StartDate < DateTime.Now )
            {
                return DiscountUseType.ExpierDate;
            }
            if (Discount.EndDate != null && Discount.EndDate <= DateTime.Now)
            {
                return DiscountUseType.ExpierDate;
            }
            if (Discount.UsableCount != null && Discount.UsableCount < 1)
            {
                return DiscountUseType.Finished;
            }
            var order = GetOrderById(orderId);

            if (_Context.UserDiscounts.Any(d => d.UserId == order.UserId && d.DiscountId == Discount.DisCountId))
            {
                return DiscountUseType.UserUsed;
            }

            order.OrderSum = (order.OrderSum * Discount.DiscountPerecnt) / 100;
            UpdateOrder(order);

            if (Discount.UsableCount != null)
            {
                Discount.UsableCount -= 1;
            }
            _Context.Discounts.Update(Discount);
            _Context.UserDiscounts.Add(new UserDiscount()
            {
                UserId = order.UserId,
                DiscountId = Discount.DisCountId,
            });
            _Context.SaveChanges();

            return DiscountUseType.Success;
        }

        public void AddDiscount(Discount discount)
        {
            _Context.Discounts.Add(discount);
            _Context.SaveChanges();
        }

        public List<Discount> GetAllDiscount()
        {
            return _Context.Discounts.ToList();
        }

        public Discount GetDiscountById(int DiscountId)
        {
            return _Context.Discounts.Find(DiscountId);
        }

        public void UpdateDiscount(Discount discount)
        {
            _Context.Discounts.Update(discount);
            _Context.SaveChanges();
        }

        public bool IsExistCode(string code)
        {
            return _Context.Discounts.Any(d => d.DiscountCode == code);
            
        }

        public bool IsUserInCourse(string username, int courseId)
        {
            int userId = _UserService.GetUserIdByUserName(username);
            return _Context.UserCourses.Any(u=>u.UserId==userId && u.CourseId==courseId);
        }
    }
}
