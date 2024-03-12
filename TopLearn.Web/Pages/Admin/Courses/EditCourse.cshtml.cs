using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopLearn.Core.DTOs.Courses;
using TopLearn.Core.Service.Interface;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Pages.Admin.Courses
{
    [Authorize]

    public class EditCourseModel : PageModel
    {
        ICourseService _CourseService;
        public EditCourseModel(ICourseService courseService)
        {
                _CourseService = courseService;
        }

        [BindProperty]
        public Course EditCourse { get; set; }

        public void OnGet(int CourseId)
        {

            EditCourse = _CourseService.GetCourseById(CourseId);

            var groups = _CourseService.GetGroupForManageCourse();
            ViewData["Groups"] = new SelectList(groups, "Value", "Text",EditCourse.GroupId);

            List<SelectListItem> SubGrouplist = new List<SelectListItem>()
            {
                new SelectListItem (){Text ="انتخاب کنید",Value=""}
            };


             SubGrouplist.AddRange( _CourseService.GetSubGroupForManageCourse(EditCourse.GroupId));
            string selectedSubGroup = null;
            if(EditCourse.SubGroup != null)
            {
                selectedSubGroup = EditCourse.SubGroup.ToString();
            }
            ViewData["SubGroups"] = new SelectList(SubGrouplist, "Value", "Text",selectedSubGroup);

            var Teachersgroups = _CourseService.GetTeachers();
            ViewData["TeachersGroups"] = new SelectList(Teachersgroups, "Value", "Text", EditCourse.TeacherId);

            var Status = _CourseService.GetStatus();
            ViewData["StatusGroups"] = new SelectList(Status, "Value", "Text", EditCourse.StatusId);

            var Level = _CourseService.GetLevels();
            ViewData["LevelGroups"] = new SelectList(Level, "Value", "Text", EditCourse.LevelId);

        }
        public IActionResult OnPost(IFormFile ImgCourseUp, IFormFile demoUp)
        {
            _CourseService.UpdateCourse(EditCourse,ImgCourseUp,demoUp);
            return RedirectToPage("Index");
        }
    }
   
}
