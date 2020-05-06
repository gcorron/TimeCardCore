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

        public Login Login(string userName, string password, string newpassword)
        {
            return QuerySingleSp<Login>("sLogin", new { userName, password, newpassword });
        }

        public IEnumerable<AppUser> GetAppUsers()
        {
            return QuerySp<AppUser>("sAppUser", null);
        }

        public int SaveAppUser(AppUser appUser)
        {
            return QuerySingleSp<int>("uAppUser", new { appUser.UserId, appUser.UserName, appUser.UserFullName, appUser.Active, appUser.Reset });
        }

        public void DeleteAppUser(int userId)
        {
            ExecuteSp("dAppUser", new { userId });
        }

        public IEnumerable<Lookup> GetUserRoles(int userId)
        {
            return QuerySp<Lookup>("sAppUserRole", new { userId });
        }

        public void DeleteUserRoles(int userId)
        {
            ExecuteSp("dAppUserRole", new { userId });
        }

        public void SaveUserRole(int userId, int roleId)
        {
            ExecuteSp("iAppUserRole", new { userId, roleId });
        }

        public Contractor GetContractor(int contractorId)
        {
            return QuerySingleSp<Contractor>("sContractor", new { contractorId });
        }

        public void SaveContractor(Contractor contractor)
        {
            ExecuteSp("uContractor", contractor);
        }

    }
}
