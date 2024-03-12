using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearnDataLayer.Entities.Order
{
    public class OrderDetail
    {
        [Key]
        public int DetailId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public int Price { get; set; }


        #region Relation
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        public Course Course { get; set; }
        #endregion
    }
}
