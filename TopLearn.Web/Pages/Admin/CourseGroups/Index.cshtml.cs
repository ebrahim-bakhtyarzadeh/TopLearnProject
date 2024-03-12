using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Service.Interface;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Pages.Admin.CourseGroups
{
    public class IndexModel : PageModel
    {
        private ICourseService _CourseService;
        public IndexModel(ICourseService courseService)
        {
            _CourseService = courseService;
        }
        public List<CourseGroup> CourseGroups { get; set; }
        public void OnGet()
        {
            CourseGroups = _CourseService.GetAllCourseGroups();
        }
    }
}
