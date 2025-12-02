using LearningManagementSystem.Data.LMSModels;
using LearningManagementSystem.Data.OtherModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Bussiness.ResourcesHandler
{
    public class ModuleResourceService : IModuleResourceService
    {
        private readonly LearningManagementContext _db;

        public ModuleResourceService(LearningManagementContext context)
        {
            _db = context;
        }

        public List<TrainingCourseModuleResource> getAllList(int id)
        {
            var ResourseList = _db.TrainingCourseModuleResources.Include(t => t.TrainingCourseModuleResourcesModule.TrainingCourseModuleTrainingCourse).Include(t => t.TrainingCourseModuleResourcesModule).Where(a => a.TrainingCourseModuleResourcesActive == true && a.TrainingCourseModuleResourcesModule.TrainingCourseModuleTrainingCourseId == id).ToList();
            return ResourseList;
        }

        public string GetCourseName(int id)
        {
            var TrainingList = _db.TrainingCourses.Where(a => a.TrainingCourseActive == true && a.TrainingCourseId == id).Select(a => a.TrainingCourseEname).FirstOrDefault();
            return TrainingList;
        }

        public async Task<List<ModuleDto>> getModulelList(int CourseId)
        {
            var list = _db.TrainingCourseModules.Where(a => a.TrainingCourseModuleActive == true && a.TrainingCourseModuleTrainingCourseId == CourseId)
                        .Select(t => new ModuleDto
                        {
                            id = t.TrainingCourseModuleId,
                            name = t.TrainingCourseModuleTrainingCourse.TrainingCourseEname + " | " + t.TrainingCourseModuleEname
                        })
                        .ToList();
            return list;
        }

        public List<TrainingCourseModuleResource> getCheckList(int id)
        {
            var ResourceList = _db.TrainingCourseModuleResources.Where(a => a.TrainingCourseModuleResourcesActive == true && a.TrainingCourseModuleResourcesModuleId == id).ToList();
            return ResourceList;
        }

        public TrainingCourseModuleResource getListId(int id)
        {
            var moduleResDetail = _db.TrainingCourseModuleResources.Include(a => a.TrainingCourseModuleResourcesModule.TrainingCourseModuleTrainingCourse).Include(a => a.TrainingCourseModuleResourcesModule.TrainingCourseModuleTrainingCourse.TrainingCourseTraining).Where(a => a.TrainingCourseModuleResourcesId == id).FirstOrDefault();
            return moduleResDetail;
        }

        public TrainingCourseModuleResource CreateResource(IFormCollection collection, IFormFile file)
        {
            var TrainingCourseModuleResources_ModuleId = collection["TrainingCourseModuleResources_ModuleId"].ToString();
            var TrainingCourseModuleResources_Type = collection["TrainingCourseModuleResources_Type"].ToString();
            var TrainingCourseModuleResources_EName = collection["TrainingCourseModuleResources_EName"].ToString();
            var TrainingCourseModuleResources_SName = collection["TrainingCourseModuleResources_SName"].ToString();
            var TrainingCourseModuleResources_TName = collection["TrainingCourseModuleResources_TName"].ToString();
            var TrainingCourseModuleResources_Sequance = collection["TrainingCourseModuleResources_Sequance"].ToString();
            var TrainingCourseModuleResources_LanguageId = collection["TrainingCourseModuleResources_LanguageId"].ToString();
            var TrainingCourseModuleResources_Description = collection["TrainingCourseModuleResources_Description"].ToString();
            var TrainingCourseModuleResources_Length = collection["TrainingCourseModuleResources_Length"].ToString();

            //var len = TimeOnly.Parse(TrainingCourseModuleResources_Length);

            TrainingCourseModuleResource resource = new TrainingCourseModuleResource();
            resource.TrainingCourseModuleResourcesModuleId = Convert.ToInt32(TrainingCourseModuleResources_ModuleId);
            resource.TrainingCourseModuleResourcesType = TrainingCourseModuleResources_Type;
            resource.TrainingCourseModuleResourcesEname = TrainingCourseModuleResources_EName;
            resource.TrainingCourseModuleResourcesSname = TrainingCourseModuleResources_SName;
            resource.TrainingCourseModuleResourcesTname = TrainingCourseModuleResources_TName;
            resource.TrainingCourseModuleResourcesDescription = TrainingCourseModuleResources_Description;
            resource.TrainingCourseModuleResourcesLanguageId = Convert.ToInt16(TrainingCourseModuleResources_LanguageId);
            resource.TrainingCourseModuleResourcesSequance = Convert.ToInt16(TrainingCourseModuleResources_Sequance);
            if (string.IsNullOrWhiteSpace(TrainingCourseModuleResources_Length))
            {
                resource.TrainingCourseModuleResourcesLength = null;
            }
            else
            {
                if (TimeOnly.TryParse(TrainingCourseModuleResources_Length, out var parsedTime))
                {
                    resource.TrainingCourseModuleResourcesLength = parsedTime;
                }
            }
            resource.TrainingCourseModuleResourcesActive = true;
            resource.TrainingCourseModuleResourcesCreatedDate = System.DateTime.Now;
            _db.TrainingCourseModuleResources.Add(resource);
            _db.SaveChanges();



            if (file != null && file.Length > 0)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                var uploadsFolder = @"E:\Attachments"; // Fixed path
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = "_" + resource.TrainingCourseModuleResourcesId + extension;
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }



            return resource;
        }
    }
}
