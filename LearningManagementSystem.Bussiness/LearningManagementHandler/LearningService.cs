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

        public async  Task<List<TrainingDto>> getTrainingList()
        {
            var list = _db.Training.Where(a => a.TrainingActive == true).Select(t => new TrainingDto
            {
                id = t.TrainingId,
                name = t.TrainingEname
            }).ToList();
            return list;
        }
    }
}
