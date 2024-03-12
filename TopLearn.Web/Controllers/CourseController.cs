using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.VisualStudio.Shell.Interop;
using SharpCompress.Archives;
using TopLearn.Core.Service;
using TopLearn.Core.Service.Interface;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Controllers
{
    public class CourseController : Controller
    {
        ICourseService _CourseService;
        IOrderService _OrderService;
        IUserService _UserService;

        public CourseController(ICourseService courseService , IOrderService orderService,IUserService userService)
        {
            _CourseService = courseService;
            _OrderService = orderService;
            _UserService = userService;

        }
        public IActionResult Index(int pageId = 1, string filter = "", string getType = "all",
        string orderByType = "date", int startPrice = 0, int endPrice = 0, List<int> SelectedGroups = null)
        {
            ViewBag.getType=getType;
            ViewBag.SelectedGruop = SelectedGroups;
            ViewBag.GetType = getType;
            ViewBag.Groups= _CourseService.GetAllCourseGroups();
            ViewBag.PageId = pageId;
            return View(_CourseService.GetCourse(pageId, filter, getType, orderByType, startPrice, endPrice, SelectedGroups, 9));
        }
        [Route("ShowCourse/{id}")]
        [HttpGet("course/fsfsfasdfsd")]

        public IActionResult ShowCourse(int id,int episode=0) 
        {
            var course = _CourseService.getCourseForShow(id);

            if(course == null)
            {
                return NotFound();
            }
            if(episode != 0 && User.Identity.IsAuthenticated)
            {
                if(course.CourseEpisodes.All(e=>e.EpisodeId != episode))
                {
                    return NotFound();
                }
                if(!course.CourseEpisodes.First(e=>e.EpisodeId == episode).IsFree)
                {
                    if(!_OrderService.IsUserInCourse(User.Identity.Name , id))
                    {
                        return NotFound();
                    }
                }
               var Episode = course.CourseEpisodes.First(e => e.EpisodeId == episode);
                ViewBag.Episode = Episode;
                string filePath = "";
                string checkFilePath = "";
                if (Episode.IsFree)
                {
                    filePath = System.IO.Path.Combine( "/CourseOnline/", Episode.EpisodeFileName.Replace(".rar", ".mp4"));
                    checkFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/courseOnline",
                        Episode.EpisodeFileName.Replace(".rar", ".mp4"));
                }
                else
                {
                    filePath = System.IO.Path.Combine( "/CourseFilesOnline", Episode.EpisodeFileName.Replace(".rar", ".mp4"));
                    checkFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseFilesOnline",
                        Episode.EpisodeFileName.Replace(".rar", ".mp4"));
                }

                if(! System.IO.File.Exists(checkFilePath))
                {
                    string targetPath = Directory.GetCurrentDirectory();
                    if (Episode.IsFree)
                    {
                        targetPath = System.IO.Path.Combine(targetPath, "wwwroot/CourseOnline");
                    }
                    else
                    {
                        targetPath = System.IO.Path.Combine(targetPath, "wwwroot/CourseFilesOnline");
                    }

                    string rarPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EpisodeFile", Episode.EpisodeFileName); 
                    var archive = ArchiveFactory.Open(rarPath);

                    var entries = archive.Entries.OrderBy(x => x.Key.Length);
                    foreach(var en in entries)
                    {
                        if(Path.GetExtension(en.Key) == ".mp4")
                        {
                            en.WriteTo(System.IO.File.Create(Path.Combine(targetPath, Episode.EpisodeFileName.Replace(".rar", ".mp4"))));
                        }
                    }
                }
                
                ViewBag.filePath = filePath;
            }

            return View(course);
        }

        [Authorize]
        public ActionResult BuyCourse(int id)
        {
           int OrderId=   _OrderService.AddOrder(User.Identity.Name, id);
            return Redirect("/UserPanel/MyOrder/ShowOrder/"+OrderId );
        }

        [Route("DownloadFile/{episodeId}")]
        public IActionResult DownloadFile(int episodeId)
        {
            var episode = _CourseService.GetEpisodeById(episodeId);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EpisodeFile", episode.EpisodeFileName);

            string fileName = episode.EpisodeFileName;

            if (episode.IsFree)
            {
                byte[] file = System.IO.File.ReadAllBytes(filePath);
                return File(file, "application/force-download", fileName);
            }

            if (User.Identity.IsAuthenticated)
            {
                if (_OrderService.IsUserInCourse(User.Identity.Name, episode.CourseId))
                {
                    byte[] file = System.IO.File.ReadAllBytes(filePath);
                    return File(file, "application/force-download", fileName);
                }
            }
            return Forbid();
        }


        [HttpPost]
        public IActionResult CreateComment(CourseComment comment)
        {
            comment.IsDeleted = false;
            comment.CreateDate = DateTime.Now;
            comment.UserId = _UserService.GetUserIdByUserName(User.Identity.Name);
            _CourseService.AddComment(comment);
            return View("ShowComment",_CourseService.GetAllComments(comment.CourseId));
        }

        public IActionResult ShowComment(int id , int pageId = 1)
        {
            return View(_CourseService.GetAllComments(id,pageId));
        }

        public IActionResult CourseVote(int id)
        {
            if (!_CourseService.IsFree(id))
            {
                if (!_OrderService.IsUserInCourse(User.Identity.Name, id))
                {
                    ViewBag.NotAccess = true;
                }
            }
            return PartialView(_CourseService.GetCourseVotes(id));
        }

        [Authorize]
        public IActionResult AddVote(int id , bool vote)
        {
            _CourseService.AddVote(_UserService.GetUserIdByUserName(User.Identity.Name),id,vote);
            return PartialView("CourseVote",_CourseService.GetCourseVotes(id));
        }

    }
}
