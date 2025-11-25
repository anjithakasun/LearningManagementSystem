using ComplaignManagementSystem.Data.Models;
using ComplaintManagementSystem.Business.Authentication;
using ComplaintManagementSystem.Business.ConncetionHandler;
using Dapper;
using LearningManagementSystem.Data.LMSModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Bussiness.UserHandler
{
    public class UserService : IUserService
    {
        private readonly _ConnectionService _connectionService;
        private readonly ADAuthentication _aDAuthentication;

        public UserService(_ConnectionService connectionService, ADAuthentication aDAuthentication)
        {
            _connectionService = connectionService;
            _aDAuthentication = aDAuthentication;
        }

        public async Task<User> ValidateUserAsync(string username, string password)
        {
            var response = await _aDAuthentication.AuthenticatewithAD(username, password);
            if (response.Status == true)
            {
                const string query = @"SELECT * FROM [dbo].[User] WHERE UserName = @UserName AND Active = 1";

                var parameters = new DynamicParameters();
                parameters.Add("@UserName", username);
                // Use the centralized connection handler for DB access
                var users = _connectionService.ReturnWithPara(query, parameters)
                                              .AsEnumerable()
                                              .Select(row => new User
                                              {
                                                  Id = row.Field<int>("Id"),
                                                  UserName = row.Field<string>("UserName"),
                                                  //Password = row.Field<string>("Password"),
                                                  Active = row.Field<bool>("Active"),
                                              })
                                              .ToList();

                var user = users.FirstOrDefault();
                if (user == null)
                    return null;
                return user;
            }
            else
            {
                return null;
            }

        }
    }
}
