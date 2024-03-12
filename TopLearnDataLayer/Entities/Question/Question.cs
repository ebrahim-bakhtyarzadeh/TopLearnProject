using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.Course;
using TopLearn.DataLayer.Entities.User;

namespace TopLearnDataLayer.Entities.Question
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required(ErrorMessage = "لطفا عنوان سوال را وارد کنید")]
        [MaxLength(400)]
        [Display(Name ="عنوان سوال")]
        public string Title { get; set; }
        [Required(ErrorMessage = "لطفا متن سوال را وارد کنید")]
        [Display(Name ="شرح سوال")]
        public string body { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
        [Required]
        public DateTime ModifiedDate { get; set; }

        #region realation
        public User User { get; set; }
        public Course Course { get; set; }
        public List<Answer> Answer { get; set; }
        #endregion
    }
}
