using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.OLE.Interop;
using TopLearn.Core.DTOs.Courses;
using TopLearn.Core.Generator;
using TopLearn.Core.Security;
using TopLearn.Core.Service.Interface;

namespace TopLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    [PermissionChecker(9)]
    public class MasterController : Controller
    {
        #region ctor
        ICourseService _CourseService;
        IUserService _UserService;
        public MasterController(ICourseService courseService ,IUserService userService)
        {
            _UserService = userService;
            _CourseService = courseService;
        }
        #endregion

        [HttpGet("mester-course")]
        public IActionResult MasterCoursesList(int id = 0)
        {
            var courses = _CourseService.GetAllCourseMaster(User.Identity.Name);
            return View(courses);
        }
        [HttpGet("master-episodes/{courseId}")]
        public IActionResult CourseEpisodeList(int courseId)
        {
            ViewBag.CourseId = courseId;

            var course=  _CourseService.GetCourseById(courseId);
           
            if(course == null)
            {
                return NotFound();
            }

            var userId = _UserService.GetUserIdByUserName(User.Identity.Name);

            if(course.TeacherId != userId)
            {
                return BadRequest();
            }

            var courseEpisode = _CourseService.GetAllCourseEpisodeMaster(courseId);
            return View(courseEpisode);
        }


        [HttpGet("add-episode/{courseId}")]
        public IActionResult AddEpisode(int courseId)
        {
            var course = _CourseService.GetCourseById(courseId);

            if (course == null)
            {
                return NotFound();
            }

            var userId = _UserService.GetUserIdByUserName(User.Identity.Name);

            if (course.TeacherId != userId)
            {
                return BadRequest();
            }
            var result = new AddEpisodeViewModel()
            {
                CourseId = courseId,
            };
            return View(result);
        }

        [HttpPost   ("add-episode/{courseId}")]
        public IActionResult AddEpisode(AddEpisodeViewModel addEpisode)
        {
            if(!ModelState.IsValid)
            {
                return View(addEpisode);
            }

            if(string.IsNullOrEmpty(addEpisode.EpisodeFileName))
            {
                return View(addEpisode);
            }

            bool result = _CourseService.AddEpisode(addEpisode, User.Identity.Name);
            if (result)
            {
                return RedirectToAction("CourseEpisodeList", "Master", new {courseId = addEpisode.CourseId});
            }

            return View(addEpisode);
        }

        public IActionResult DropzoneTarget(List<IFormFile> files, int courseId)
        {

            if (files != null && files.Any())
            {
                foreach (var file in files)
                {
                    string  fileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(file.FileName);
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EpisodeFile/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                        
                    string  finalPath = path + fileName; 
    
                    using (var stream = new FileStream (finalPath,FileMode.Create))
                    {
                        file.CopyTo (stream);
                    }
                    return new JsonResult(new {data = fileName ,status ="Success" });
                }
            }

            return new JsonResult(new {  status = "Error" });
        }
    }
}
