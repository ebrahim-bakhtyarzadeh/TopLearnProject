using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.DTOs.Question;
using TopLearnDataLayer.Entities.Question;

namespace TopLearn.Core.Service.Interface
{
    public interface IForumService
    {
        #region Question
        int AddQuestion(Question question);

        public ShowQuestionViewModel ShowQuestion(int questionId);
        public IEnumerable<Question> GetQuestions(int? courseId, string filter = "");

        #endregion

        #region Answer

        void AddAnswer (Answer answer);
        public void ChangeIsTrueAnswer(int questionId,int answerId);
        
        #endregion
    }
}
