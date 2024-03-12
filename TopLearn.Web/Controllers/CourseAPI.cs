using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TopLearnDataLayer.Context;

namespace TopLearn.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseAPI : ControllerBase
    {
        TopLearnContext _context;
        public CourseAPI(TopLearnContext Context)
        {
            _context = Context;
        }
        [Produces("application/json")]
        [HttpGet("Search")]
        public async Task<IActionResult> Search()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var CourseTitle= _context.Courses.Where(c=>c.CourseTitle.Contains(term)).Select(c=>c.CourseTitle).ToList();
                return Ok(CourseTitle);
            }
            catch 
            {
                return BadRequest();
            }
        }
    }
}
