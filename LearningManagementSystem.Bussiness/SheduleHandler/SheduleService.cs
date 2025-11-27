using LearningManagementSystem.Data.LMSModels;
using LearningManagementSystem.Data.OtherModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Bussiness.SheduleHandler
{
    public class SheduleService : ISheduleService
    {
        private readonly LearningManagementContext _db;

        public SheduleService(LearningManagementContext context)
        {
            _db = context;
        }

        public List<TrainingShedule> getAllList(int id)
        {
            var TrainingSheeduleList = _db.TrainingShedules.Include(t => t.TrainingSheduleTrainingCourse).Include(t => t.TrainingSheduleTrainingCourse.TrainingCourseTraining).Where(a => a.TrainingSheduleActive == true && a.TrainingSheduleTrainingCourseId == id).ToList();
            return TrainingSheeduleList;
        }

        public TrainingShedule CreateShedule(IFormCollection collection)
        {
            var TrainingShedule_TrainingCourseId = collection["TrainingShedule_TrainingCourseId"].ToString();
            var TrainingShedule_TrainerId = collection["TrainingShedule_TrainerId"].ToString();
            var TrainingShedule_Name = collection["TrainingShedule_Name"].ToString();
            var TrainingShedule_StartDate = collection["TrainingShedule_StartDate"].ToString();
            var TrainingShedule_EndDate = collection["TrainingShedule_EndDate"].ToString();
            var TrainingShedule_ParticipantCount = collection["TrainingShedule_ParticipantCount"].ToString();
            var TrainingShedule_Description = collection["TrainingShedule_Description"].ToString();

            TrainingShedule shedule = new TrainingShedule();
            shedule.TrainingSheduleTrainingCourseId = Convert.ToInt32(TrainingShedule_TrainingCourseId);
            shedule.TrainingSheduleTrainerId = Convert.ToInt32(TrainingShedule_TrainerId);
            shedule.TrainingSheduleName = TrainingShedule_Name;
            shedule.TrainingSheduleStartDate = Convert.ToDateTime(TrainingShedule_StartDate);
            shedule.TrainingSheduleEndDate = Convert.ToDateTime(TrainingShedule_EndDate);
            shedule.TrainingSheduleParticipantCount = Convert.ToInt16(TrainingShedule_ParticipantCount);
            shedule.TrainingSheduleDescription = TrainingShedule_Description;
            shedule.TrainingSheduleActive = true;
            shedule.TrainingSheduleIsComplete = false;
            shedule.TrainingSheduleCreatedDate = System.DateTime.Now;
            _db.TrainingShedules.Add(shedule);
            _db.SaveChanges();
            return shedule;
        }

        public async Task<List<TrainingCourse>> getCourselList(int trainingId)
        {
            var CourseList = _db.TrainingCourses.Where(a => a.TrainingCourseActive == true && a.TrainingCourseTrainingId == trainingId).ToList();
            return CourseList;
        }

        public async Task<List<TrainerDto>> getTrainerlList()
        {

            var TrainerList = _db.Trainers.Where(a => a.TrainerActive == true)
                                .Select(t => new TrainerDto
                                {
                                    id = t.TrainerId,
                                    name = t.TrainerEpf + " | " + t.TrainerFname + " " + t.TrainerLname
                                })
                                .ToList();

            return TrainerList;
        }

        public TrainingShedule getListId(int id)
        {
            var sheduleList = _db.TrainingShedules.Where(a => a.TrainingSheduleId == id).FirstOrDefault();
            return sheduleList;
        }

        public TrainingShedule updateShedule(IFormCollection collection)
        {
            var id = collection["Id"].ToString();
            var TrainingShedule_TrainingCourseId = collection["TrainingShedule_ETrainingCourseId"].ToString();
            var TrainingShedule_TrainerId = collection["TrainingShedule_ETrainerId"].ToString();
            var TrainingShedule_Name = collection["TrainingShedule_EName"].ToString();
            var TrainingShedule_StartDate = collection["TrainingShedule_EStartDate"].ToString();
            var TrainingShedule_EndDate = collection["TrainingShedule_EEndDate"].ToString();
            var TrainingShedule_ParticipantCount = collection["TrainingShedule_EParticipantCount"].ToString();
            var TrainingShedule_Description = collection["TrainingShedule_EDescription"].ToString();

            TrainingShedule shedule = _db.TrainingShedules.Where(a => a.TrainingSheduleId == Convert.ToInt32(id)).FirstOrDefault();

            shedule.TrainingSheduleTrainingCourseId = Convert.ToInt32(TrainingShedule_TrainingCourseId);
            shedule.TrainingSheduleTrainerId = Convert.ToInt32(TrainingShedule_TrainerId);
            shedule.TrainingSheduleName = TrainingShedule_Name;
            shedule.TrainingSheduleStartDate = Convert.ToDateTime(TrainingShedule_StartDate);
            shedule.TrainingSheduleEndDate = Convert.ToDateTime(TrainingShedule_EndDate);
            shedule.TrainingSheduleParticipantCount = Convert.ToInt16(TrainingShedule_ParticipantCount);
            shedule.TrainingSheduleDescription = TrainingShedule_Description;
            _db.SaveChanges();
            return shedule;
        }

        public TrainingShedule DeleteShedule(int id)
        {
            TrainingShedule Shedule = _db.TrainingShedules.Where(a => a.TrainingSheduleId == id).FirstOrDefault();
            Shedule.TrainingSheduleActive = false;
            _db.SaveChanges();
            return Shedule;
        }

        public async Task<List<CourseDto>> getCourseList(int trainingId)
        {
            var list = _db.TrainingCourses.Where(a => a.TrainingCourseActive == true && a.TrainingCourseTrainingId == trainingId)
                        .Select(t => new CourseDto
                        {
                            id = t.TrainingCourseId,
                            name = t.TrainingCourseTraining.TrainingEname + " | " + t.TrainingCourseEname
                        })
                        .ToList();
            return list;
        }
        public TrainingCourse getCourseDetail(int id)
        {
            var TrainingList = _db.TrainingCourses.Include(t => t.TrainingCourseTraining).Where(a => a.TrainingCourseActive == true && a.TrainingCourseId == id).First();
            return TrainingList;
        }
    }
}
