using LearningManagementSystem.Data.LMSModels;
using LearningManagementSystem.Data.OtherModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Bussiness.LearningManagementHandler
{
    public interface ILearningService
    {
        public Task<List<TrainingDto>> getTrainingList();
        public Task<List<CourseDto>> getCourseList(int trainingId);
    }
}
