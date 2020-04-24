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
    public class LookupRepo:BaseRepo
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

    }
}
