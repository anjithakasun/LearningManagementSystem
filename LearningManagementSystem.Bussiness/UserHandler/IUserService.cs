using ComplaignManagementSystem.Data.Models;
using LearningManagementSystem.Data.LMSModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Bussiness.UserHandler
{
    public interface IUserService
    {
        Task<User> ValidateUserAsync(string username, string password);
    }
}
