using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.Service.Interface;

namespace TopLearn.Web.ViewComponents
{
    public class LatesCourseViewComponent:ViewComponent
    {
        private ICourseService _CourseService;
        public LatesCourseViewComponent(ICourseService courseService)
        {
            _CourseService = courseService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            return await Task.FromResult((IViewComponentResult)View("LatesCourse", _CourseService.GetCourse()));
        }
    }
}
