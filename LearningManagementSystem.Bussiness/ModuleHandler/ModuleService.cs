using LearningManagementSystem.Data.LMSModels;
using LearningManagementSystem.Data.OtherModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Bussiness.ModuleHandler
{
    public class ModuleService : IModuleService
    {
        private readonly LearningManagementContext _db;

        public ModuleService(LearningManagementContext context)
        {
            _db = context;
        }

        public List<TrainingCourseModule> getAllList(int id)
        {
            var ModuleList = _db.TrainingCourseModules.Include(t => t.TrainingCourseModuleTrainingCourse).Include(t => t.TrainingCourseModuleTrainingCourse.TrainingCourseTraining).Where(a => a.TrainingCourseModuleActive == true && a.TrainingCourseModuleTrainingCourse.TrainingCourseTrainingId == id).ToList();
            return ModuleList;
        }

        public List<TrainingCourseModule> getCheckList(int id)
        {
            var ModuleList = _db.TrainingCourseModules.Include(t => t.TrainingCourseModuleTrainingCourse).Include(t => t.TrainingCourseModuleTrainingCourse.TrainingCourseTraining).Where(a => a.TrainingCourseModuleActive == true && a.TrainingCourseModuleTrainingCourseId == id).ToList();
            return ModuleList;
        }

        public TrainingCourseModule CreateModule(IFormCollection collection)
        {
            var TrainingCourseModule_TrainingCourseId = collection["TrainingCourseModule_TrainingCourseId"].ToString();
            var TrainingCourseModule_EName = collection["TrainingCourseModule_EName"].ToString();
            var TrainingCourseModule_SName = collection["TrainingCourseModule_SName"].ToString();
            var TrainingCourseModule_TName = collection["TrainingCourseModule_TName"].ToString();
            var TrainingCourseModule_Sequance = collection["TrainingCourseModule_Sequance"].ToString();
            var TrainingCourseModule_IsQuiz = collection["TrainingCourseModule_IsQuiz"].ToString();
            var TrainingCourseModule_QuizTypeId = collection["TrainingCourseModule_QuizTypeId"].ToString();
            var TrainingCourseModule_Description = collection["TrainingCourseModule_Description"].ToString();

            TrainingCourseModule module = new TrainingCourseModule();
            module.TrainingCourseModuleTrainingCourseId = Convert.ToInt32(TrainingCourseModule_TrainingCourseId);
            module.TrainingCourseModuleEname = TrainingCourseModule_EName;
            module.TrainingCourseModuleSname = TrainingCourseModule_SName;
            module.TrainingCourseModuleTname = TrainingCourseModule_TName;
            module.TrainingCourseModuleDescription = TrainingCourseModule_Description;
            module.TrainingCourseModuleSequance = Convert.ToInt16(TrainingCourseModule_Sequance);
            module.TrainingCourseModuleIsQuiz = (TrainingCourseModule_IsQuiz == "Yes");
            module.TrainingCourseModuleQuizTypeId = TrainingCourseModule_IsQuiz == "Yes"
                ? Convert.ToInt16(TrainingCourseModule_QuizTypeId)
                : (short?)null;

            module.TrainingCourseModuleActive = true;
            module.TrainingCourseModuleCreatedDate = System.DateTime.Now;
            _db.TrainingCourseModules.Add(module);
            _db.SaveChanges();
            return module;
        }

        public TrainingCourseModule DeleteModule(int id)
        {
            TrainingCourseModule module = _db.TrainingCourseModules.Where(a => a.TrainingCourseModuleId == id).FirstOrDefault();
            module.TrainingCourseModuleActive = false;
            _db.SaveChanges();
            return module;
        }

        public TrainingCourseModule getCourseDetail(int id)
        {
            throw new NotImplementedException();
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

        public async Task<List<QuizTypeDto>> getAuizTypeList()
        {
            var list = _db.QuizTypes.Where(a => a.QuizTypeActive == true)
                        .Select(t => new QuizTypeDto
                        {
                            id = t.QuizTypeId,
                            name = t.QuizTypeType
                        })
                        .ToList();
            return list;
        }

        public async Task<List<TrainingCourse>> getCourselList(int trainingId)
        {
            var CourseList = _db.TrainingCourses.Where(a => a.TrainingCourseActive == true && a.TrainingCourseTrainingId == trainingId).ToList();
            return CourseList;
        }

        public TrainingCourseModule getListId(int id)
        {
            var moduleDetail = _db.TrainingCourseModules.Include(a => a.TrainingCourseModuleQuizType).Include(a => a.TrainingCourseModuleTrainingCourse).Include(a => a.TrainingCourseModuleTrainingCourse.TrainingCourseTraining).Where(a => a.TrainingCourseModuleId == id).FirstOrDefault();
            return moduleDetail;
        }

        public TrainingCourseModule updateModule(IFormCollection collection)
        {
            var id = collection["Id"].ToString();
            var TrainingCourseModule_TrainingCourseId = collection["TrainingCourseModule_ETrainingCourseId"].ToString();
            var TrainingCourseModule_EName = collection["TrainingCourseModule_EEName"].ToString();
            var TrainingCourseModule_SName = collection["TrainingCourseModule_ESName"].ToString();
            var TrainingCourseModule_TName = collection["TrainingCourseModule_ETName"].ToString();
            var TrainingCourseModule_Sequance = collection["TrainingCourseModule_ESequance"].ToString();
            var TrainingCourseModule_IsQuiz = collection["TrainingCourseModule_EIsQuiz"].ToString();
            var TrainingCourseModule_QuizTypeId = collection["TrainingCourseModule_EQuizTypeId"].ToString();
            var TrainingCourseModule_Description = collection["TrainingCourseModule_EDescription"].ToString();

            TrainingCourseModule module = _db.TrainingCourseModules.Where(a => a.TrainingCourseModuleId == Convert.ToInt32(id)).FirstOrDefault();
            module.TrainingCourseModuleTrainingCourseId = Convert.ToInt32(TrainingCourseModule_TrainingCourseId);
            module.TrainingCourseModuleEname = TrainingCourseModule_EName;
            module.TrainingCourseModuleSname = TrainingCourseModule_SName;
            module.TrainingCourseModuleTname = TrainingCourseModule_TName;
            module.TrainingCourseModuleDescription = TrainingCourseModule_Description;
            module.TrainingCourseModuleSequance = Convert.ToInt16(TrainingCourseModule_Sequance);
            module.TrainingCourseModuleIsQuiz = (TrainingCourseModule_IsQuiz == "Yes");
            module.TrainingCourseModuleQuizTypeId = TrainingCourseModule_IsQuiz == "Yes"
                ? Convert.ToInt16(TrainingCourseModule_QuizTypeId)
                : (short?)null; module.TrainingCourseModuleActive = true;

            _db.SaveChanges();
            return module;
        }

        public String GetTrainingName(int id)
        {
            var TrainingList = _db.TrainingCourses.Include(t => t.TrainingCourseTraining).Where(a => a.TrainingCourseActive == true && a.TrainingCourseTrainingId == id).Select(a => a.TrainingCourseTraining.TrainingEname).FirstOrDefault();
            return TrainingList;
        }
    }
}
