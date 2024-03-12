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
    public class CourseVote
    {
        [Key]
        public int VoteId { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public bool Vote { get; set; }
        public DateTime VoteDate { get; set; } = DateTime.Now;


        #region Relation
        public Course Course { get; set; }
        public User.User User { get; set; }
        #endregion
    }
}
