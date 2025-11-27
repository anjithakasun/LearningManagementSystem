using LearningManagementSystem.Data.LMSModels;
using LearningManagementSystem.Data.OtherModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Bussiness.SheduleHandler
{
    public interface ISheduleService
    {
        public Task<List<TrainingCourse>> getCourselList(int trainingId);
        public Task<List<TrainerDto>> getTrainerlList();
        public List<TrainingShedule> getAllList(int id);
        public TrainingShedule CreateShedule(IFormCollection collection);
        public TrainingShedule getListId(int id);
        public TrainingShedule updateShedule(IFormCollection collection);
        public TrainingShedule DeleteShedule(int id);
        public Task<List<CourseDto>> getCourseList(int trainingId);
        public TrainingCourse getCourseDetail(int id);
    }
}
