using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.Convertors;
using TopLearn.Core.DTOs.Courses;
using TopLearn.Core.Generator;
using TopLearn.Core.Security;
using TopLearn.Core.Service.Interface;
using TopLearn.DataLayer.Entities.Course;
using TopLearnDataLayer.Context;


namespace TopLearn.Core.Service
{
    public class CourseService : ICourseService
    {
        private TopLearnContext _context;
        IUserService _userService;
        public CourseService(TopLearnContext context, IUserService userService)
        {
            _userService = userService;
            _context = context;
        }
        public List<CourseGroup> GetAllCourseGroups()
        {
            return _context.courseGroups.Include(c => c.CourseGroups).ToList();
        }

        public List<SelectListItem> GetGroupForManageCourse()
        {
            return _context.courseGroups.Where(c => c.ParentId == null).Select(g => new SelectListItem()
            {
                Text = g.GroupTitle,
                Value = g.GroupId.ToString(),
            }).ToList();
        }


        public List<SelectListItem> GetSubGroupForManageCourse(int GroupId)
        {
            return _context.courseGroups.Where(p => p.ParentId == GroupId).Select(g => new SelectListItem()
            {
                Text = g.GroupTitle,
                Value = g.GroupId.ToString(),
            }).ToList();
        }

        public List<SelectListItem> GetTeachers()
        {
            List<SelectListItem> a = _context.UserRoles.Where(r => r.RoleId == 3).Include(r => r.User).Select(u => new SelectListItem()
            {
                Value = u.UserId.ToString(),
                Text = u.User.UserName
            }).ToList();
            return a;
        }

        public List<SelectListItem> GetLevels()
        {
            return _context.CourseLevels.Select(u => new SelectListItem()
            {
                Text = u.LevelTitle,
                Value = u.LevelId.ToString(),
            }).ToList();
        }

        public List<SelectListItem> GetStatus()
        {
            return _context.CourseStatuses.Select(u => new SelectListItem()
            {
                Text = u.StatusTitle,
                Value = u.StatusId.ToString(),
            }).ToList();
        }








        #region Course
        public int AddCourse(Course course, IFormFile imgCourse, IFormFile courseDemo)
        {
            course.CreateDate = DateTime.Now;
            course.CourseImageName = "no-photo.jpg";

            if (imgCourse != null && imgCourse.IsImage())
            {


                course.CourseImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(imgCourse.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/image", course.CourseImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgCourse.CopyTo(stream);
                }
                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/thumb", course.CourseImageName);

                //ImageConvertor imgResizer = new ImageConvertor();
                //imgResizer.Image_resize(imagePath, thumbPath, 250);
                using (var image = SixLabors.ImageSharp.Image.Load(imagePath))
                {
                    // resize picture 
                    image.Mutate(i => i.Resize(new ResizeOptions
                    {
                        Size = new SixLabors.ImageSharp.Size(345, 100),
                        Mode = ResizeMode.Max

                    }));
                    //فشرده سازی تصویر با کیفیت 80 
                    image.Save(thumbPath, new JpegEncoder { Quality = 100 });
                }

            }


            if (courseDemo != null)
            {
                course.DemoFileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(courseDemo.FileName);
                string demoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/demo", course.DemoFileName);
                using (var stream = new FileStream(demoPath, FileMode.Create))
                {
                    courseDemo.CopyTo(stream);
                }
                course.DemoFileName = course.DemoFileName;
            }


            _context.Courses.Add(course);

            _context.SaveChanges();
            return course.CourseId;
        }

        public List<ShowCourseForAdminViewModel> GetAllCourseForAdmin()
        {
            return _context.Courses.Select(c => new ShowCourseForAdminViewModel()
            {
                CourseId = c.CourseId,
                ImageName = c.CourseImageName,
                Title = c.CourseTitle,
                EpisodeCount = c.CourseEpisodes.Count,
            }).ToList();
        }

        public Course GetCourseById(int Id)
        {
            return _context.Courses.Find(Id);
        }

        public void UpdateCourse(Course course, IFormFile imgCourse, IFormFile courseDemo)
        {
            course.UpdateDate = DateTime.Now;

            if (imgCourse != null && imgCourse.IsImage())
            {
                if (course.CourseImageName != "no-photo.jpg")
                {
                    string DeleteimagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/image", course.CourseImageName);
                    if (File.Exists(DeleteimagePath))
                    {
                        File.Delete(DeleteimagePath);
                    }

                    string deletethumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/thumb", course.CourseImageName);
                    if (File.Exists(deletethumbPath))
                    {
                        File.Delete(deletethumbPath);
                    }
                }
                course.CourseImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(imgCourse.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/image", course.CourseImageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgCourse.CopyTo(stream);
                }


                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/thumb", course.CourseImageName);




                using (var image = SixLabors.ImageSharp.Image.Load(imagePath))
                {
                    // resize picture 
                    image.Mutate(i => i.Resize(new ResizeOptions
                    {
                        Size = new SixLabors.ImageSharp.Size(345, 150),
                        Mode = ResizeMode.Max

                    }));

                    image.Save(thumbPath, new PngEncoder());
                }

            }
            if (courseDemo != null)
            {
                if (course.DemoFileName != null)
                {
                    string deleteDemoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/demo", course.DemoFileName);
                    if (File.Exists(deleteDemoPath))
                    {
                        File.Delete(deleteDemoPath);
                    }
                }
                course.DemoFileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(courseDemo.FileName);
                string demoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/demo", course.DemoFileName);
                using (var stream = new FileStream(demoPath, FileMode.Create))
                {
                    courseDemo.CopyTo(stream);
                }
            }

            _context.Courses.Update(course);
            _context.SaveChanges();





        }

        public int AddEpisode(CourseEpisode episode, IFormFile EpisodeFile)
        {
            episode.EpisodeFileName = EpisodeFile.FileName;

            string FileEpisodePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EpisodeFile", episode.EpisodeFileName);
            using (var stream = new FileStream(FileEpisodePath, FileMode.Create))
            {
                EpisodeFile.CopyTo(stream);
            }

            _context.CourseEpisodes.Add(episode);
            _context.SaveChanges();
            return episode.EpisodeId;
        }

        public bool CheckExistFile(string filename)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EpisodeFile", filename);
            return Path.Exists(path);
        }

        public List<CourseEpisode> GetListOfEpisodeCoures(int courseId)
        {
            return _context.CourseEpisodes.Where(p => p.CourseId == courseId).ToList();
        }

        public CourseEpisode GetEpisodeById(int id)
        {
            return _context.CourseEpisodes.Find(id);
        }

        public void EditEpisode(CourseEpisode episode, IFormFile episodefile)
        {
            if (episodefile != null)
            {
                string deletefilepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EpisodeFile", episode.EpisodeFileName);
                File.Delete(deletefilepath);

                episode.EpisodeFileName = episodefile.FileName;
                string filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EpisodeFile", episode.EpisodeFileName);
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    episodefile.CopyTo(stream);
                }

            }
            _context.CourseEpisodes.Update(episode);
            _context.SaveChanges();
        }

        public Tuple<List<ShowCourseItemListViewModel>, int> GetCourse(int pageId = 1, string filter = "", string getType = "all",
            string orderByType = "date", int startPrice = 0, int endPrice = 0, List<int> SelectedGroups = null, int take = 0)
        {
            if (take == 0)
                take = 8;
            IQueryable<Course> result = _context.Courses;
            if (!string.IsNullOrEmpty(filter))
            {
                result = result.Where(c => c.CourseTitle.Contains(filter) || c.Tags.Contains(filter));
            }
            switch (getType)
            {
                case "all":
                    {
                        break;
                    }
                case "buy":
                    {
                        result = result.Where(r => r.CoursePrice != 0);
                        break;
                    }

                case "free":
                    {
                        result = result.Where(r => r.CoursePrice == 0);
                        break;
                    }
            }
            switch (orderByType)
            {
                case "date":
                    {
                        result = result.OrderByDescending(c => c.CreateDate);
                        break;
                    }
                case "updatedate":
                    {
                        result = result.OrderByDescending(c => c.UpdateDate);
                        break;
                    }
            }
            if (startPrice > 0)
            {
                result = result.Where(c => c.CoursePrice >= startPrice);
            }
            if (endPrice > 0)
            {
                result = result.Where(c => c.CoursePrice <= endPrice);
            }
            if (SelectedGroups != null && SelectedGroups.Any())
            {
                foreach (int groupId in SelectedGroups)
                {
                    result = result.Where(c => c.GroupId == groupId || c.SubGroup == groupId);
                }
            }

            int skip = (pageId - 1) * take;

            int pageCount = result.Include(c => c.CourseEpisodes).Select(c => new ShowCourseItemListViewModel()
            {
                courseId = c.CourseId,
                Title = c.CourseTitle,
                imageName = c.CourseImageName,
                Price = c.CoursePrice,

            }).Count() / take;

            var query = result.Include(c => c.CourseEpisodes).Select(c => new ShowCourseItemListViewModel()
            {
                courseId = c.CourseId,
                Title = c.CourseTitle,
                imageName = c.CourseImageName,
                Price = c.CoursePrice,
                CourseEpisode = c.CourseEpisodes


            }).Skip(skip).Take(take).ToList();


            return Tuple.Create(query, pageCount);
        }

        public Course getCourseForShow(int courseId)
        {
            return _context.Courses.Include(c => c.CourseEpisodes)
                .Include(c => c.CourseLevel).Include(c => c.CourseStatus)
                .Include(c => c.User).Include(c => c.UserCourses)
                .FirstOrDefault(c => c.CourseId == courseId);
        }

        public void AddComment(CourseComment comment)
        {
            _context.CourseComment.Add(comment);
            _context.SaveChanges();
        }

        public Tuple<List<CourseComment>, int> GetAllComments(int CourseId, int pageId = 1)
        {
            int take = 5;
            int skip = (pageId - 1) * take;
            int pageCount = _context.CourseComment.Where(c => !c.IsDeleted && c.CourseId == CourseId).Count() / take;
            if ((pageCount % 2) != 0)
            {
                pageCount++;
            }
            return Tuple.Create(_context.CourseComment.Include(c => c.User)
                .Where(c => !c.IsDeleted && c.CourseId == CourseId).Skip(skip).Take(take).OrderByDescending(c => c.CreateDate).ToList(), pageCount);
        }

        public List<Course> GetPopularCourse()
        {
            return _context.Courses.Include(od => od.OrderDetails).Include(c => c.CourseEpisodes)
                .Where(c => c.OrderDetails.Any())
                .OrderByDescending(c => c.OrderDetails.Count)
                .Take(8).ToList();
        }

        public void AddGroup(CourseGroup group)
        {
            _context.courseGroups.Add(group);
            _context.SaveChanges();
        }

        public void UpdateGroup(CourseGroup group)
        {
            _context.courseGroups.Update(group);
            _context.SaveChanges();
        }

        public CourseGroup GetById(int GroupId)
        {
            return _context.courseGroups.Find(GroupId);
        }

        public void AddVote(int userId, int CourseId, bool vote)
        {
            var UserVote = _context.CourseVotes.FirstOrDefault(v => v.UserId == userId && v.CourseId == CourseId);
            if (UserVote != null)
            {
                UserVote.Vote = vote;
                _context.Update(UserVote);
                _context.SaveChanges();

            }
            else
            {
                UserVote = new CourseVote()
                {
                    UserId = userId,
                    CourseId = CourseId,
                    Vote = vote
                };
                _context.Add(UserVote);
                _context.SaveChanges();
            }
           
        }

        public Tuple<int, int> GetCourseVotes(int courseId)
        {
            var votes = _context.CourseVotes.Where(v=>v.CourseId == courseId).Select(v=>v.Vote).ToList();
            return Tuple.Create(votes.Count(c => c), votes.Count(c => !c));
        }

        public bool IsFree(int courseId)
        {
            return _context.Courses.Where(c=>c.CourseId == courseId).Select(c=>c.CoursePrice).FirstOrDefault()==0;
        }

        #endregion

        #region CourseMaster
        public List<Course> GetAllCourseMaster(string username)
        {
            int userId = _userService.GetUserIdByUserName(username);
            var Courses = _context.Courses.Where(c => c.TeacherId == userId).Include(c => c.CourseEpisodes).Include(c => c.CourseStatus).ToList();
            return Courses;
        }

        public List<CourseEpisode> GetAllCourseEpisodeMaster(int courseId)
        {
            return _context.CourseEpisodes.Where(c=>c.CourseId == courseId).ToList();
        }

        public bool AddEpisode(AddEpisodeViewModel addEpisode ,string username )
        {
            var course = GetCourseById(addEpisode.CourseId);

            var userId = _context.Users.FirstOrDefault(s=>s.UserName == username).UserId;

            if (course == null || course.TeacherId != userId) 
            {
                return false;
            }

            var episode = new CourseEpisode()
            {
                CourseId = course.CourseId,
                IsFree = addEpisode.IsFree,
                EpisodeTitle = addEpisode.EpisodeTitle, 
                EpisodeTime = addEpisode.EpisodeTime,
                EpisodeFileName = addEpisode.EpisodeFileName,
            };
            _context.Add(episode);
            _context.SaveChanges();
            return true;
        }
        #endregion

    }
}
