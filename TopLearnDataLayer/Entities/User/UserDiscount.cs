using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.User;
using TopLearnDataLayer.Entities.Order;

namespace TopLearn.DataLayer.Entities.User
{
    public class UserDiscount
    {
        [Key]
        public int UD_Id { get; set; }
        public int UserId { get; set; }
        public int DiscountId { get; set; }
        public User User { get; set; }
        public Discount Discount { get; set; }
    }
}
