using LearningManagementSystem.Data.LMSModels;
using LearningManagementSystem.Data.OtherModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Bussiness.ResourcesHandler
{
    public interface IModuleResourceService
    {
        public Task<List<ModuleDto>> getModulelList(int CourseId);
        public List<TrainingCourseModuleResource> getAllList(int id);
        public List<TrainingCourseModuleResource> getCheckList(int id);
        public TrainingCourseModuleResource getListId(int id);
        public TrainingCourseModuleResource CreateResource(IFormCollection collection, IFormFile file);
        public TrainingCourseModuleResource UpdateResource(IFormCollection collection, IFormFile file);
        public String GetCourseName(int id);
        public bool DeleteAttachment(int id);
        public TrainingCourseModuleResource DeleteResource(int id);


    }
}
