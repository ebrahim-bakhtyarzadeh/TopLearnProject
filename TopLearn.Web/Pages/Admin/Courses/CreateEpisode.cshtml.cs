using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs.Courses;
using TopLearn.Core.Service.Interface;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Pages.Admin.Courses
{
    [Authorize]

    public class CreateEpisodeModel : PageModel
    {
        ICourseService _CourseService;
        public CreateEpisodeModel(ICourseService courseService)
        {
            _CourseService = courseService;
        }

        [BindProperty]
        public CourseEpisodeViewModel CourseEpisode { get; set; }
        public void OnGet(int id)
        {
            CourseEpisode = new CourseEpisodeViewModel();
             CourseEpisode.CourseId = id;
            
        }
        public IActionResult OnPost(IFormFile EpisodeFile)
        {

            if (!ModelState.IsValid || EpisodeFile == null )
                return Page();
            CourseEpisode Episode = new CourseEpisode()
            {
                CourseId = CourseEpisode.CourseId,
               
                EpisodeTime = CourseEpisode.EpisodeTime,
                EpisodeTitle = CourseEpisode.EpisodeTitle,
                IsFree = CourseEpisode.IsFree,

            }; 
            if(_CourseService.CheckExistFile(EpisodeFile.FileName))
            {
                ViewData["IsExistFile"] = true;
                return Page();
            }
            _CourseService.AddEpisode(Episode, EpisodeFile);
            return Redirect("/Admin/Courses/IndexEpisode/" + CourseEpisode.CourseId);
        }
    }
}
