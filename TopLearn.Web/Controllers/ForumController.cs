using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TopLearn.Core.Service.Interface;
using TopLearnDataLayer.Entities.Question;

namespace TopLearn.Web.Controllers
{
    public class ForumController : Controller
    {
        public readonly IUserService _UserService;
        private readonly IForumService _ForumService;
        public ForumController(IForumService forumService ,IUserService userService)
        {
            _ForumService = forumService;
            _UserService = userService;
        }

        public IActionResult Index(int?courseId,string filter="")
        {
            ViewBag.CourseId = courseId;
          return View(_ForumService.GetQuestions(courseId, filter));
        }


        #region CreateQuestion
        [Authorize]
        public IActionResult CreateQuestion(int id)
        {
            Question question = new Question()
            {
                CourseId = id,
            };

            return View(question);
        }

        [HttpPost]

        public IActionResult CreateQuestion(Question question)
        {
            if (!ModelState.IsValid)
            {
                return View(question);
            }
            question.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            int questionId = _ForumService.AddQuestion(question);
            return Redirect($"/Forum/ShowQuestion/{questionId}");

        }
        #endregion

        #region ShowQuestion
        
        public IActionResult ShowQuestion(int Id)

        {

            return View(_ForumService.ShowQuestion(Id)); 
        }
        [Authorize]
        public IActionResult Answer(int id,string body )
        {
            var sanitizer = new HtmlSanitizer();
            
            body = sanitizer.Sanitize(body, "https://localhost:44392/");
            if(!string.IsNullOrEmpty(body))
            {
                _ForumService.AddAnswer(new Answer()
                {
                    BodyAnswer = body,
                    CreateDate = DateTime.Now,
                    UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString()),
                    QuestionId = id
                });
            }

            return RedirectToAction("ShowQuestion", new { Id = id });
        }
        [Authorize]
        public IActionResult SelectIsTrueAnswer(int questionId ,int answerId)
        {
            int currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            var question = _ForumService.ShowQuestion(questionId);
            if(question.Question.UserId == currentUserId)
            {
                _ForumService.ChangeIsTrueAnswer(questionId, answerId);
            }
            return RedirectToAction("ShowQuestion", new { Id = questionId });
        }
        #endregion

    }
}
