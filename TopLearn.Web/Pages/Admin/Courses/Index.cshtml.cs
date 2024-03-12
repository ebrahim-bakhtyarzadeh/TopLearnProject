using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs.Courses;
using TopLearn.Core.Service.Interface;

namespace TopLearn.Web.Pages.Admin.Courses
{
    [Authorize]
    public class IndexModel : PageModel
    {
        ICourseService _courseService;
        public IndexModel(ICourseService courseService)
        {
            _courseService = courseService;
        }
        public List<ShowCourseForAdminViewModel> ListCourse { get; set; }
        public void OnGet()
        {
            ListCourse = _courseService.GetAllCourseForAdmin();
        }
    }
}
