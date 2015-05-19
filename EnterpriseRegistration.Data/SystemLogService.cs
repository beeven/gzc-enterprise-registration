using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Data
{
    public class SystemLogService : ISystemLogService
    {
        static Models.EntRegDb db = new Models.EntRegDb();
        public void Save(Models.SystemLog log)
        {
            db.SystemLogs.Add(log);
            db.SaveChanges();
        }

        public List<Models.SystemLog> GetAll()
        {
            return db.SystemLogs.AsNoTracking().OrderByDescending(x => x.CreateTime).ToList();
        }


        public List<Models.SystemLog> GetByTypes(string types)
        {
            List<string> typelist = new List<string>(types.Split('|'));
            return db.SystemLogs.AsNoTracking().Where(e => typelist.Contains(e.type.ToString())).OrderByDescending(x => x.CreateTime).ToList();
        } 
    }
}
