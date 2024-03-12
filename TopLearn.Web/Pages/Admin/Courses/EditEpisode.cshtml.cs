using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.TextManager.Interop;
using TopLearn.Core.DTOs.Courses;
using TopLearn.Core.Service.Interface;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Pages.Admin.Courses
{
    [Authorize]

    public class EditEpisodeModel : PageModel
    {
        ICourseService _CourseService;
        public EditEpisodeModel(ICourseService courseService)
        {
            _CourseService= courseService;
        }
        [BindProperty]
        public CourseEpisode CourseEpisode { get; set; }

        public void OnGet(int id)
        {
             CourseEpisode = _CourseService.GetEpisodeById(id);
          
            

        }

        public IActionResult OnPost(IFormFile EpisodeFile)
        {

            if (!ModelState.IsValid )
                return Page();
           
            if(EpisodeFile!= null)
            {
                if (_CourseService.CheckExistFile(EpisodeFile.FileName))
                {
                    ViewData["IsExistFile"] = true;
                    return Page();
                }
            }


           
            _CourseService.EditEpisode(CourseEpisode, EpisodeFile);
            return Redirect("/Admin/Courses/IndexEpisode/" + CourseEpisode.CourseId);
        }
    }
}
