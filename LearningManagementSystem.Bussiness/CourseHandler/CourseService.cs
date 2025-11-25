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
            throw new NotImplementedException();
        }

        public TrainingCourse DeleteCourse(int id)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
