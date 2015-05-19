using EnterpriseRegistration.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Data
{
    public class MSSQLRegInfoService : IRegInfoService
    {
        static Models.EntRegDb db = new Models.EntRegDb();
        public MSSQLRegInfoService()
        {

        }

        public int Save(Models.RegInfo info)
        {
           // using (Models.EntRegDb db = new Models.EntRegDb())
            //{
                db.RegInfoes.Add(info);
                db.SaveChanges();
                return info.Id;
            //}
        }

        public List<Models.RegInfo> GetAll()
        {
            List<Models.RegInfo> result;
            //using (Models.EntRegDb db = new Models.EntRegDb())
            //{
                result= db.RegInfoes.AsNoTracking().OrderByDescending(x => x.ReceivedDate).ToList();
            //}
            return result;
        }

        public List<Models.RegInfo> GetAllByUserCustomCode(List<string> roleIds, string customCode)
        {
            //using (Models.EntRegDb db = new Models.EntRegDb())
            //{
            List<string> customMapCodes = db.CustomRights.AsNoTracking().Where(x => roleIds.Contains(x.CustomCode)).Select(x => x.AvailableCustomCode).ToList();

            List<string> customCodes = db.CustomMap.AsNoTracking().Where(x => x.TopCustomCode == customCode).Select(e => e.MapCode).ToList();
            customMapCodes.AddRange(customCodes);
            //List<string> customNames = db.Customs.AsNoTracking().Where(x => customMapCodes.Contains(x.CustomCode)).Select(x => x.CustomName).ToList();
            List<int> availableIds = db.Registrations.AsNoTracking().Where(x => customMapCodes.Contains(x.CustomsName)).Select(x => x.AttchID).ToList();
            return db.RegInfoes.AsNoTracking().Where(e => availableIds.Contains(e.Id)).OrderByDescending(x => x.ReceivedDate).ToList();
            //}
       }

        public Models.RegInfo Get(int id)
        {
            Models.RegInfo result;
            //using(Models.EntRegDb db = new Models.EntRegDb())
            //{
                result=db.RegInfoes.AsNoTracking().Where<Models.RegInfo>(x => x.Id == id).FirstOrDefault();
            //}
            return result;
           // return db.RegInfoes.Find(id);
            
        }

        public void Update(Models.RegInfo info)
        {
            Models.RegInfo model = db.RegInfoes.Where(p => p.Id == info.Id).FirstOrDefault();
            model.isGetEntAccess = info.isGetEntAccess;
            model.isRecordPass = info.isRecordPass;
            model.IsReplied = info.IsReplied;
            model.ReceivedDate = info.ReceivedDate;
            model.RecordNum = info.RecordNum;
            model.RepliedBody = info.RepliedBody;
            model.RepliedDate = info.RepliedDate;
            model.Subject = info.Subject;
            model.Body = info.Body;
            model.From = info.From;
            model.GetEntAccessDate = info.GetEntAccessDate;
            db.SaveChanges();
           
        }


        public void SetReplied(int id, string content)
        {
            //using (Models.EntRegDb db = new Models.EntRegDb())
            //{
                var regOld = db.RegInfoes.Where(x => x.Id == id).SingleOrDefault();
                regOld.IsReplied = true;
                regOld.RepliedBody = content;
                regOld.RepliedDate = DateTime.Now;
                //RegInfo info = new RegInfo()
                //{
                //    Id = id,
                //    IsReplied = true,
                //    RepliedBody=content,
                //    RepliedDate = DateTime.Now
                //};

                //db.RegInfoes.Attach(info);
                //var entry = db.Entry(info);
                //entry.Property(e => e.RepliedDate).IsModified = true;
                //entry.Property(e => e.IsReplied).IsModified = true;
                //entry.Property(e => e.RepliedBody).IsModified = true;

                db.SaveChanges();
            //}
           
        }


        #region 删除
        /// <summary>
        /// 删除当前申请
        /// </summary>
        /// <param name="projId"></param>
        /// <returns></returns>
        public void Delete(int id)
        {
            //using (Models.EntRegDb db = new Models.EntRegDb())
            //{
                RegInfo reg = db.RegInfoes.Find(id);
                if (reg != null)
                {
                    db.RegInfoes.Remove(reg);
                    db.SaveChanges();
                }
            //}
        }
        #endregion
    }
}
