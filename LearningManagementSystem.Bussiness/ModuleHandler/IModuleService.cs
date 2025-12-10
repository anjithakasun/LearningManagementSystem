using LearningManagementSystem.Data.LMSModels;
using LearningManagementSystem.Data.OtherModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Bussiness.ModuleHandler
{
    public interface IModuleService
    {
        public Task<List<TrainingCourse>> getCourselList(int trainingId);
        public List<TrainingCourseModule> getAllList(int id);
        public List<TrainingCourseModule> getCheckList(int id);
        public TrainingCourseModule CreateModule(IFormCollection collection);
        public TrainingCourseModule getListId(int id);
        public TrainingCourseModule updateModule(IFormCollection collection);
        public TrainingCourseModule DeleteModule(int id);
        public Task<List<CourseDto>> getCourseList(int trainingId);
        public Task<List<QuizTypeDto>> getAuizTypeList();
        public TrainingCourseModule getCourseDetail(int id);
        public String GetTrainingName(int id);
    }
}
