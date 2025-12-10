using LearningManagementSystem.Data.LMSModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Bussiness.CourseHandler
{
    public interface ICourseService
    {
        public Task<List<Training>> getTrainingList();
        public List<TrainingCourse> getAllList(int id);
        public TrainingCourse CreateCourse(IFormCollection collection);
        public TrainingCourse getListId(int id);
        public TrainingCourse updateCourse(IFormCollection collection);
        public TrainingCourse DeleteCourse(int id);
        public String GetTrainingName(int id);
    }
}
