using EnvDTE;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.DTOs.Courses;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Core.Service.Interface
{
    public interface ICourseService
    {
        #region Group

        List<CourseGroup> GetAllCourseGroups();
        List<SelectListItem> GetGroupForManageCourse();
        List<SelectListItem> GetSubGroupForManageCourse(int GroupId);
        List<SelectListItem> GetTeachers();
        List<SelectListItem> GetLevels ();
        List<SelectListItem> GetStatus();

        CourseGroup GetById (int GroupId);
        void AddGroup(CourseGroup group);
        void UpdateGroup (CourseGroup group);

        #endregion

        #region Course
        int AddCourse(Course course, IFormFile imgCourse, IFormFile courseDemo);

        List<ShowCourseForAdminViewModel> GetAllCourseForAdmin();

        Course GetCourseById(int Id);
        void UpdateCourse (Course course, IFormFile imgCourse, IFormFile courseDemo);

        Tuple<List<ShowCourseItemListViewModel>,int> GetCourse(int pageId = 1, string filter = "", string getType="all", string orderByType = "date",int startPrice=0,int endPrice = 0,List<int>SelectedGroups=null, int take = 0);

        Course getCourseForShow(int courseId);

        public List<Course> GetPopularCourse();
        void AddVote(int userId, int CourseId, bool Vote);

        Tuple<int,int> GetCourseVotes(int courseId);
        bool IsFree(int courseId);

        List<Course> GetAllCourseMaster(string username);
        List<CourseEpisode> GetAllCourseEpisodeMaster(int courseId);

        public bool AddEpisode(AddEpisodeViewModel addEpisode, string username);
        #endregion

        #region Episode
        List<CourseEpisode> GetListOfEpisodeCoures(int courseId);
        public bool CheckExistFile(string filename);
        int AddEpisode(CourseEpisode episode, IFormFile EpisodeFile);
        CourseEpisode GetEpisodeById( int id);
        void EditEpisode(CourseEpisode episode, IFormFile episodefile);


        #endregion

        #region Commetnt
        void AddComment(CourseComment comment);
        Tuple<List<CourseComment>,int> GetAllComments(int CourseId , int pageId=1);


        #endregion
    }
}
