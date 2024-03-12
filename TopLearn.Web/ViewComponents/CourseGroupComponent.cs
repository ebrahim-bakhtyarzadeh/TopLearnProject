using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.Service.Interface;

namespace TopLearn.Web.ViewComponents
{
    
    public class CourseGroupViewComponent : ViewComponent 
    {
        private ICourseService _CourseService;
        public CourseGroupViewComponent(ICourseService courseService)
        {
            _CourseService = courseService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult) View ("CourseGroup" ,_CourseService.GetAllCourseGroups()));
        }
    }
}
