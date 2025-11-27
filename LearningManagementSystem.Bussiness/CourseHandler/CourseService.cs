using LearningManagementSystem.Data.LMSModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Bussiness.CourseHandler
{
    public class CourseService : ICourseService
    {
        private readonly LearningManagementContext _db;

        public CourseService(LearningManagementContext context)
        {
            _db = context;
        }

        public TrainingCourse CreateCourse(IFormCollection collection)
        {
            var TrainingCourse_TrainingId = collection["TrainingCourse_TrainingId"].ToString();
            var TrainingCourse_EName = collection["TrainingCourse_EName"].ToString();
            var TrainingCourse_SName = collection["TrainingCourse_SName"].ToString();
            var TrainingCourse_TName = collection["TrainingCourse_TName"].ToString();
            var TrainingCourse_Description = collection["TrainingCourse_Description"].ToString();
            var TrainingCourse_Sequance = collection["TrainingCourse_Sequance"].ToString();

            TrainingCourse course = new TrainingCourse();
            course.TrainingCourseTrainingId = Convert.ToInt32(TrainingCourse_TrainingId);
            course.TrainingCourseEname = TrainingCourse_EName;
            course.TrainingCourseSname = TrainingCourse_SName;
            course.TrainingCourseTname = TrainingCourse_TName;
            course.TrainingCourseDescription = TrainingCourse_Description;
            course.TrainingCourseSequance = Convert.ToInt16(TrainingCourse_Sequance);
            course.TrainingCourseActive = true;
            course.TrainingCourseCreatedDate = System.DateTime.Now;
            _db.TrainingCourses.Add(course);
            _db.SaveChanges();
            return course;
        }

        public List<TrainingCourse> getAllList(int id)
        {
            var TrainingList = _db.TrainingCourses.Include(t => t.TrainingCourseTraining).Where(a => a.TrainingCourseActive == true && a.TrainingCourseTrainingId == id).ToList();
            return TrainingList;
        }

        public TrainingCourse getListId(int id)
        {
            var courseDetail = _db.TrainingCourses.Where(a => a.TrainingCourseId == id).FirstOrDefault();
            return courseDetail;
        }

        public async Task<List<Training>> getTrainingList()
        {
            var TrainingList = _db.Training.Where(a => a.TrainingActive == true).ToList();
            return TrainingList;
        }

        public TrainingCourse updateCourse(IFormCollection collection)
        {
            var id = collection["Id"].ToString();
            var TrainingCourse_TrainingId = collection["TrainingCourse_ETrainingId"].ToString();
            var TrainingCourse_EName = collection["TrainingCourse_EEName"].ToString();
            var TrainingCourse_SName = collection["TrainingCourse_ESName"].ToString();
            var TrainingCourse_TName = collection["TrainingCourse_ETName"].ToString();
            var TrainingCourse_Description = collection["TrainingCourse_EDescription"].ToString();
            var TrainingCourse_Sequance = collection["TrainingCourse_ESequance"].ToString();


            TrainingCourse course = _db.TrainingCourses.Where(a => a.TrainingCourseId == Convert.ToInt32(id)).FirstOrDefault();
            course.TrainingCourseTrainingId = Convert.ToInt32(TrainingCourse_TrainingId);
            course.TrainingCourseEname = TrainingCourse_EName;
            course.TrainingCourseSname = TrainingCourse_SName;
            course.TrainingCourseTname = TrainingCourse_TName;
            course.TrainingCourseDescription = TrainingCourse_Description;
            _db.SaveChanges();
            return course;
        }

        public TrainingCourse DeleteCourse(int id)
        {
            TrainingCourse Course = _db.TrainingCourses.Where(a => a.TrainingCourseId == id).FirstOrDefault();
            Course.TrainingCourseActive = false;
            _db.SaveChanges();
            return Course;
        }

    }
}
