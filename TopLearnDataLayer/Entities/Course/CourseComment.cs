using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.Course;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.DataLayer.Entities.Course
{
    public class CourseComment
    {
        [Key]
        public int CommentId { get; set; }
        public int CourseId { get; set; }
        public int UserId { get; set; }
        [MaxLength(700)]
        public string Comment {  get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAdminRead { get; set; }



        #region Relation
        public Course Course { get; set; }
        public User.User User {  get; set; } 
        #endregion
    }
}
