using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseRegistration.Data.Models;

namespace EnterpriseRegistration.Data
{
    public class CustomRightService:ICustomRightsService
    {
        static Models.EntRegDb db = new Models.EntRegDb();
        public List<Models.CustomRights> GetByCustomCode(string customCode)
        {
            return db.CustomRights.AsNoTracking().Where(x => x.CustomCode == customCode).ToList();
        }

        public void UpdateCustomRights(string customCode, List<string> AvailableCustomCodes)
        {
            List<CustomRights> insertRights = new List<CustomRights>();
            foreach (string availableCode in AvailableCustomCodes)
            {
                CustomRights cRight = new CustomRights()
                {
                    CustomCode=customCode,
                    AvailableCustomCode=availableCode
                };
                insertRights.Add(cRight);
            }
            db.CustomRights.RemoveRange(db.CustomRights.Where(x => x.CustomCode == customCode).ToList().AsEnumerable());
            db.CustomRights.AddRange(insertRights);
            db.SaveChanges();
        }
    }
}
