using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Data
{
    public class CustomService:ICustomsService
    {
        static Models.EntRegDb db = new Models.EntRegDb();

        public List<Models.Customs> GetAll()
        {
            List<Models.Customs> list = db.Customs.AsNoTracking().ToList();

            return list;
        }


        public void Update(string id, string contact, string tel)
        {
            int parseId = int.Parse(id);
            var infoOld = db.Customs.Where(x => x.Id == parseId).SingleOrDefault();
            infoOld.Contact = contact;
            infoOld.Tel = tel;
            //Models.Customs info = new Models.Customs()
            //{
            //    Id = int.Parse(id),
            //    Contact = contact,
            //    Tel = tel
            //};

            //db.Customs.Attach(info);
            //var entry = db.Entry(info);
            //entry.Property(e => e.Contact).IsModified = true;
            //entry.Property(e => e.Tel).IsModified = true;
            db.SaveChanges();
        }
    }
}
