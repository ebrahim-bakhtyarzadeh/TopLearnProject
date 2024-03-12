using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TopLearn.Core.Service.Interface;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Pages.Admin.Courses
{
    [Authorize]

    public class IndexEpisodeModel : PageModel
    {
        ICourseService _CourseService;
        public IndexEpisodeModel(ICourseService courseService)
        {
            _CourseService = courseService;
        }
       
        public List<CourseEpisode> CourseEpisode { get; set; }
        public void OnGet(int CourseId)
        {
            ViewData["CourseId"] = CourseId;
            CourseEpisode = _CourseService.GetListOfEpisodeCoures(CourseId);
        }
    }
}
