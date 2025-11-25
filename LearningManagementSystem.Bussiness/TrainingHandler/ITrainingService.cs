using LearningManagementSystem.Data.LMSModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Bussiness.TrainingHandler
{
    public interface ITrainingService
    {
        public List<Training> getAllList();
        public Training CreateTraining(IFormCollection collection);
        public Training getListId(int id);
        public Training updateTraining(IFormCollection collection);
        public Training DeleteTraining(int id);

    }
}
