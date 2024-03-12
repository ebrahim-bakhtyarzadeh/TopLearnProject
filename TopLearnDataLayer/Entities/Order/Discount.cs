﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.User;

namespace TopLearnDataLayer.Entities.Order
{
    public class Discount
    {
        [Key]
        public int DisCountId { get; set; }
        [Display(Name ="کد")]
        [Required (ErrorMessage ="لطفا {0} را وارد کنید")]      
        [MaxLength(150)]

        public string DiscountCode { get; set;}
        [Display(Name = "درصد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int DiscountPerecnt {  get; set; }
        public int? UsableCount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set;}


        #region Relation
        public List<UserDiscount> UserDiscounts { get; set; }
        #endregion
    }
}
