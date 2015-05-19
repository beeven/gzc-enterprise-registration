using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Data
{
    public class CustomMapService:ICustomMapService
    {
        static Models.EntRegDb db = new Models.EntRegDb();
        public List<Models.CustomMap> GetAll()
        {
            return db.CustomMap.AsNoTracking().ToList();
        }


        public List<Models.CustomMap> GetByMapCode(string mapcode)
        {
            return db.CustomMap.AsNoTracking().Where(x=>x.MapCode==mapcode).ToList();
        }


        public string GetMapCodeByCustomCode(string customcode)
        {
            Models.CustomMap cm = db.CustomMap.AsNoTracking().Where(x => x.TopCustomCode == customcode).SingleOrDefault();
            if (cm != null)
                return cm.MapCode;
            else
                return "";
        }
    }
}
