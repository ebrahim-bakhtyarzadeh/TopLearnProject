using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Service.Interface;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Pages.Admin.CourseGroups
{
    public class CreateGroupModel : PageModel
    {
        ICourseService _CourseService;
        public CreateGroupModel(ICourseService CourseService)
        {
            _CourseService = CourseService;
        }
        [BindProperty]
        public CourseGroup CourseGroup { get; set; }
        public void OnGet(int? id)
        {
            CourseGroup = new CourseGroup()
            {
                ParentId = id,
            };
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _CourseService.AddGroup(CourseGroup);
            return RedirectToPage("Index");

        }
    }
}
