using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Core.DTOs.Courses
{
    public class ShowCourseItemListViewModel
    {
        public int courseId {  get; set; }
        public string Title { get; set; }
        public string imageName { get; set; }
        public int Price { get; set; }
        public List<CourseEpisode> CourseEpisode { get; set; }
    }
}
