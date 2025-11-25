using LearningManagementSystem.Data.LMSModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LearningManagementSystem.Bussiness.TrainingHandler
{    
    public class TrainingService : ITrainingService
    {
        private readonly LearningManagementContext _db;

        public TrainingService(LearningManagementContext context)
        {
            _db = context;
        }

        public List<Training> getAllList()
        {
                var TrainingList = _db.Training.Where(a => a.TrainingActive == true).ToList();
                return TrainingList;
        }

        public Training CreateTraining(IFormCollection collection)
        {
            var Training_EName = collection["Training_EName"].ToString();
            var Training_SName = collection["Training_SName"].ToString();
            var Training_TName = collection["Training_TName"].ToString();
            var Training_Description = collection["Training_Description"].ToString();

            Training training = new Training();
            training.TrainingEname = Training_EName;
            training.TrainingSname = Training_SName;
            training.TrainingTname = Training_TName;
            training.TrainingDescription = Training_Description;
            training.TrainingActive = true;
            training.TrainingCreatedDate = System.DateTime.Now;
            _db.Training.Add(training);
            _db.SaveChanges();
            return training;
        }

        public Training getListId(int id)
        {
            Training trainnig = _db.Training.Where(a => a.TrainingId == id).FirstOrDefault();
            return trainnig;
        }

        public Training updateTraining(IFormCollection collection)
        {
            var id = collection["Id"].ToString();
            var Training_EName = collection["Training_EEName"].ToString();
            var Training_SName = collection["Training_ESName"].ToString();
            var Training_TName = collection["Training_ETName"].ToString();
            var Training_Description = collection["Training_EDescription"].ToString();


            Training training = _db.Training.Where(a => a.TrainingId == Convert.ToInt32(id)).FirstOrDefault();
            training.TrainingEname = Training_EName;
            training.TrainingSname = Training_SName;
            training.TrainingTname = Training_TName;
            training.TrainingDescription = Training_Description;
            _db.SaveChanges();
            return training;
        }

        public Training DeleteTraining(int id)
        {
            Training trainnig = _db.Training.Where(a => a.TrainingId == id).FirstOrDefault();
            trainnig.TrainingActive = false;
            _db.SaveChanges();
            return trainnig;
        }
    }
}
