using System;
using System.Collections.Generic;
using System.Text;
using TimeCard.Domain;

namespace TimeCard.Repo.Repos
{
    public class AppUserRepo : BaseRepo
    {
        public AppUserRepo(string connectionString) : base(connectionString)
        {

        }

        public Login Login(string userName, string password)
        {
            return QuerySingleSp<Login>("sLogin", new { userName, password });
        }

        public void UpdateLogin(string userName, string password)
        {
            ExecuteSp("uLogin", new { userName, password });
        }

        public IEnumerable<string> GetRoles(int userId)
        {
            return QuerySp<string>("sAppUserRole", new { userId });
        }

        public IEnumerable<AppUser> GetAppUsers()
        {
            return QuerySp<AppUser>("sAppUser", null);
        }

        public void UpdateAppUser(AppUser appUser)
        {
            ExecuteSp("uAppUser", appUser);
        }

        public void DeleteAppUser(int userId)
        {
            ExecuteSp("dAppUser", new { userId });
        }


    }
}
