using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.Core.DTOs.Courses
{
    public class CourseInsertViewModel
    {
        public string CourseTitle { get; set; }
        public int GroupId { get; set; }
        public int SubId { get; set; }
        public int TeacherId { get; set; }
       public string CourseDescription { get; set; }
       public int CoursePrice { get; set; }
       public string Tags { get; set; }
       public int StatusId { get; set; }
      public  int LevelId { get; set; }
    }

    public class ShowCourseForAdminViewModel
    {
        public int CourseId { get; set; }
        public string ImageName { get; set; }
        public string Title { get; set; }
        public int EpisodeCount { get; set; }
        

    }
    public class  EditCourseViewModel
    {
        public string CourseTitle { get; set; }
        public int GroupId { get; set; }
        public int SubId { get; set; }
        public int TeacherId { get; set; }
        public string CourseDescription { get; set; }
        public int CoursePrice { get; set; }
        public string Tags { get; set; }
        public int StatusId { get; set; }
        public int LevelId { get; set; }
        public string ImageName { get; set; }
    }
    public class CourseEpisodeViewModel
    {
        public int CourseId { get; set; }
        [Display(Name = "عنوان بخش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(400, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string EpisodeTitle { get; set; }

        [Display(Name = "زمان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public TimeSpan EpisodeTime { get; set; }
        
        [Display(Name = "رایگان")]
        public bool IsFree { get; set; }

    }
}
