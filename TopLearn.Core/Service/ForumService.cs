using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.DTOs.Question;
using TopLearn.Core.Service.Interface;
using TopLearnDataLayer.Context;
using TopLearnDataLayer.Entities.Question;
using TopLearnDataLayer.Migrations;

namespace TopLearn.Core.Service
{
    public class ForumService : IForumService
    {
        private readonly TopLearnContext _Context;
        private readonly IUserService _UserService;
        public ForumService(TopLearnContext Context, IUserService userService)
        {
            _Context = Context;
            _UserService = userService;

        }
        public int AddQuestion(Question question)
        {
            question.CreateTime = DateTime.Now;
            question.ModifiedDate = DateTime.Now;

            _Context.Questions.Add(question);
            _Context.SaveChanges();
            return question.QuestionId;
        }
        public IEnumerable<Question> GetQuestions(int? courseId, string filter = "")
        {
            IQueryable<Question> query = _Context.Questions.Where(q => EF.Functions.Like(q.Title, $"%{filter}%"));
            if (courseId != null)
            {
                query = query.Where(q=>q.CourseId == courseId);
            }
            return query.Include(q=>q.User).Include(q=>q.Course).ToList();
        }

        public ShowQuestionViewModel ShowQuestion(int questionId)
        {
            ShowQuestionViewModel question = new ShowQuestionViewModel();
            question.Question = _Context.Questions.Include(q => q.User).FirstOrDefault(q => q.QuestionId == questionId);
            question.Answers = _Context.Answers.Include(a => a.User).Where(q => q.QuestionId == questionId).ToList();
            return question;

        }





        public void AddAnswer(Answer answer)
        {
            _Context.Answers.Add(answer);
            _Context.SaveChanges();
        }

        public void ChangeIsTrueAnswer(int questionId, int answerId)
        {
            var answers = _Context.Answers.Where(a => a.QuestionId == questionId);
            foreach (var ans in answers)
            {
                ans.IsCurrect = false;
                if (ans.AnswerId == answerId)
                {
                    ans.IsCurrect = true;
                }
            }
            _Context.UpdateRange(answers);
            _Context.SaveChanges();

        }


    }
}
