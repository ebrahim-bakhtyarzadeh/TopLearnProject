using EnvDTE;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopLearn.Core.DTOs.Courses;
using TopLearn.Core.Security;
using TopLearn.Core.Service;
using TopLearn.Core.Service.Interface;
using TopLearn.DataLayer.Entities.Course;


namespace TopLearn.Web.Pages.Admin.Courses
{
    [Authorize]

    public class CreateCourseModel : PageModel
    {
        ICourseService _CourseService;
        public CreateCourseModel(ICourseService CourseService)
        {
            _CourseService = CourseService;
        }

        [BindProperty]
        //public  Course Course { get; set; }
        public CourseInsertViewModel CourseForIsert { get; set; }
        public void OnGet()
        {
            var groups = _CourseService.GetGroupForManageCourse();
            ViewData["Groups"] = new SelectList(groups, "Value", "Text");

            var Subgroups = _CourseService.GetSubGroupForManageCourse(int.Parse( groups.First().Value));
            ViewData["SubGroups"] = new SelectList(Subgroups, "Value", "Text");

            var Teachersgroups = _CourseService.GetTeachers();
            ViewData["TeachersGroups"] = new SelectList(Teachersgroups, "Value", "Text");

            var Status = _CourseService.GetStatus();
            ViewData["StatusGroups"] = new SelectList(Status, "Value", "Text");
           
            var Level = _CourseService.GetLevels();
            ViewData["LevelGroups"] = new SelectList(Level, "Value", "Text");
        }
        public IActionResult OnPost(IFormFile ImgCourseUp ,IFormFile demoUp)
        {

            if (!ModelState.IsValid)
                return Page();

            Course course = new Course() 
            {
                CourseTitle = CourseForIsert.CourseTitle,
                GroupId = CourseForIsert.GroupId,
                SubGroup = CourseForIsert.SubId,
                TeacherId = CourseForIsert.TeacherId,
                CourseDescription = CourseForIsert.CourseDescription,
                CoursePrice= CourseForIsert.CoursePrice,
                Tags = CourseForIsert.Tags,
                StatusId = CourseForIsert.StatusId,
                LevelId= CourseForIsert.LevelId,
            };

            _CourseService.AddCourse(course, ImgCourseUp,demoUp);

            return RedirectToPage("index");
            
        }
    }
}
