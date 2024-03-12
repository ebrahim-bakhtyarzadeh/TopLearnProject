using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Service.Interface;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Pages.Admin.CourseGroups
{
    public class EditGroupModel : PageModel
    {
        ICourseService _CourseService;
        public EditGroupModel(ICourseService CourseService)
        {
            _CourseService = CourseService;
        }
        [BindProperty]
        public CourseGroup CourseGroup { get; set; }
        public void OnGet(int id)
        {
            CourseGroup = _CourseService.GetById(id);   
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _CourseService.UpdateGroup(CourseGroup);
            return RedirectToPage("Index");
                
        }
    }
}
