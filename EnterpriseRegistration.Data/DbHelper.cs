using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Infrastructure;
using System.Data;

namespace EnterpriseRegistration.Data
{
    public class DbHelper<T>
    {
        public static List<T> ExcuteSql(System.Data.Entity.DbContext dbContext, string sqlString, List<SqlParameter> paraList)
        {
            var cmd = dbContext.Database.Connection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = sqlString;
            cmd.CommandTimeout = 900000000;
            if (paraList != null)
            {
                cmd.Parameters.AddRange(paraList.ToArray());
            }
            List<T> result = new List<T>();
            try
            {
                dbContext.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                var items = ((IObjectContextAdapter)dbContext).ObjectContext.Translate<T>(reader);
                result = items.ToList();
            }
            finally
            {
                dbContext.Database.Connection.Close();
            }
            return result;
        }

    }
}
