using LearningManagementSystem.Data.LMSModels;
using LearningManagementSystem.Data.OtherModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Bussiness.LearningManagementHandler
{
    public class LearningService : ILearningService
    {
        private readonly LearningManagementContext _db;

        public LearningService(LearningManagementContext context)
        {
            _db = context;
        }


        public async Task<List<TrainingDto>> getTrainingList()
        {
            var list = _db.Training.Where(a => a.TrainingActive == true).Select(t => new TrainingDto
            {
                id = t.TrainingId,
                name = t.TrainingEname
            }).ToList();
            return list;
        }


        public async Task<List<CourseDto>> getCourseList(int trainingId)
        {
            var list = _db.TrainingCourses.Where(a => a.TrainingCourseActive == true && a.TrainingCourseTrainingId == trainingId)
                        .Select(t => new CourseDto
                        {
                            id = t.TrainingCourseId,
                            name = t.TrainingCourseEname
                        })
                        .ToList();
            return list;
        }

        public async Task<List<ModuleDto>> getModuleList(int courseId)
        {
            var list = _db.TrainingCourseModules.Where(a => a.TrainingCourseModuleActive == true && a.TrainingCourseModuleTrainingCourseId == courseId)
                        .Select(t => new ModuleDto
                        {
                            id = t.TrainingCourseModuleId,
                            name = t.TrainingCourseModuleEname
                        })
                        .ToList();
            return list;
        }
    }
}
