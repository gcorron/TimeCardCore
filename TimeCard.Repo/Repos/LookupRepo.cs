using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeCard.Domain;

namespace TimeCard.Repo.Repos
{
    public class LookupRepo : BaseRepo
    {
        public LookupRepo(string connectionString) : base(connectionString)
        {

        }

        public Lookup GetLookupByDescr(string group, string descr)
        {
            return QuerySingleSp<Lookup>("sLookup", new { group, descr });
        }

        public Lookup GetLookupByVal(string group, string val)
        {
            return QuerySingleSp<Lookup>("sLookup", new { group, val });
        }

        public IEnumerable<Lookup> GetLookups(string group, string addFirstRow = null)
        {
            var data = QuerySp<Lookup>("sLookup", new { group });
            if (addFirstRow != null)
            {
                return new Lookup[] { new Lookup { Id = 0, Descr = addFirstRow } }.Union(data);
            }
            else
            {
                return data;
            }
        }
        public IEnumerable<Lookup> GetLookups(int groupId)
        {
            return QuerySp<Lookup>("sLookup", new { groupId });
        }


        public IEnumerable<LookupGroup> GetGroups()
        {
            return Enumerable.Repeat(new LookupGroup { GroupId = 0, Descr = "- Select -" }, 1).Union(QuerySp<LookupGroup>("sLookupGroup", null));
        }

        public  void SaveLookup(Lookup lookup)
        {
            ExecuteSp("uLookup", lookup);
        }
        public void DeleteLookup(int id)
        {
            ExecuteSp("dLookup",new { id });
        }

        public IEnumerable<string> GetRolesForUser(string userName)
        {
            return QuerySp<string>("sLookupRolesForUser", new { userName });
        }

    }
}
