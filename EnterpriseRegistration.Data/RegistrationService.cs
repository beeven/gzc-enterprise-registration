using EnterpriseRegistration.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace EnterpriseRegistration.Data
{
    public class RegistrationService : IRegistrationService
    {
        static Models.EntRegDb db = new Models.EntRegDb();

        /// <summary>
        /// 分析pdf
        /// </summary>
        /// <param name="RegistrationStream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Dictionary<string, object> AnalysisPDF(byte[] context)
        {
            Dictionary<string, object> dict = PDFHelper.ReadForm(context);
            return dict;
        }

        /// <summary>
        /// 插入PDF
        /// </summary>
        /// <param name="dic"></param>
        public void InsertPDF(Dictionary<string, object> dic, int AttchID)
        {
            //实例化转换对象
            Registration reg = new Registration();
            //赋值
            EntityHelper<Registration>.SetValue(reg, dic);
            reg.ID = Guid.NewGuid();
            reg.AttchID = AttchID;
            //添加要保存的对象
            db.Registrations.Add(reg);
            //执行
            db.SaveChanges();
        }

        /// <summary>
        /// 分析Excel
        /// </summary>
        /// <param name="RegistrationStream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public DataTable AnalysisExcel(Stream RegistrationStream, string fileName)
        {
            ExcelHelper excel = new ExcelHelper();
            DataTable dt = excel.ExcelToDataTable(RegistrationStream, fileName, true);
            return dt;
        }

        /// <summary>
        /// 将数据导入到数据库
        /// </summary>
        /// <param name="RegistrationStream"></param>
        public void InsertExcel(DataTable dt, int AttchID)
        {
            try
            {
                string CustomsName = dt.Rows[0][2].ToString();
                //获取CustomsName的Code
                string CustomCode = db.Customs.AsNoTracking().Where(x => x.CustomName == CustomsName).Select(p => p.CustomCode).FirstOrDefault().ToString();

                Registration reg = new Registration();


                reg.ID = Guid.NewGuid();
                reg.EnterPriseName = dt.Rows[0][0].ToString();
                reg.EnterPriseAbbreviation = dt.Rows[0][1].ToString();
                reg.CustomsName = CustomCode;
                reg.CustomsID = dt.Rows[0][3].ToString();
                reg.OnlineFlag = int.Parse(dt.Rows[0][4].ToString());
                reg.DirectFlag = int.Parse(dt.Rows[0][5].ToString());
                reg.ManagementFlag = int.Parse(dt.Rows[0][6].ToString());
                reg.TransactionFlag = int.Parse(dt.Rows[0][7].ToString());
                reg.LogisticsFlag = int.Parse(dt.Rows[0][8].ToString());
                reg.PayFlag = int.Parse(dt.Rows[0][9].ToString());
                reg.Abroad = int.Parse(dt.Rows[0][10].ToString());
                reg.OrgCode = dt.Rows[0][11].ToString();
                reg.LegalName = dt.Rows[0][12].ToString();
                reg.LegalID = dt.Rows[0][13].ToString();
                reg.LicenseID = dt.Rows[0][14].ToString();
                reg.TaxCode = dt.Rows[0][15].ToString();
                reg.EnterPrisePhone = dt.Rows[0][16].ToString();
                reg.EnterPriseContact = dt.Rows[0][17].ToString();
                reg.EnterPriseAddress = dt.Rows[0][18].ToString();
                reg.TransactionName = dt.Rows[0][19].ToString();
                reg.TransactionWebsite = dt.Rows[0][20].ToString();
                reg.LogisticsWebsite = dt.Rows[0][21].ToString();
                reg.LegalCopyFlag = int.Parse(dt.Rows[0][22].ToString());
                reg.TaxCopyFlag = int.Parse(dt.Rows[0][23].ToString());
                reg.OrgCopyFlag = int.Parse(dt.Rows[0][24].ToString());
                reg.PayCopyFlag = int.Parse(dt.Rows[0][25].ToString());
                reg.LicenseCopyFlag = int.Parse(dt.Rows[0][26].ToString());
                reg.Remark = dt.Rows[0][27].ToString();
                reg.RecordID = dt.Rows[0][28].ToString();
                reg.UnitsFlag = int.Parse(dt.Rows[0][29].ToString());
                reg.AttnMobile = dt.Rows[0][30].ToString();
                reg.AttnEmail = dt.Rows[0][31].ToString();
                reg.OperateFlag = int.Parse(dt.Rows[0][32].ToString());
                reg.ICPCode = dt.Rows[0][33].ToString();
                reg.AttchID = AttchID;
                //插入到数据库

                db.Registrations.Add(reg);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                //向上抛出异常
                throw ex;
            }

        }

        public List<Models.Registration> GetAll()
        {
            return db.Registrations.AsNoTracking().ToList();
        }

        public Models.Registration Get(Guid id)
        {
            return db.Registrations.Find(id);
        }

        public Models.Registration GetByAttchID(int AttchID)
        {
            return db.Registrations.AsNoTracking().Where(x => x.AttchID == AttchID).FirstOrDefault();
        }

        /// <summary>
        /// 根据企业名称查找当前企业备案信息
        /// </summary>
        /// <param name="CustomsName">企业名称</param>
        /// <returns></returns>
        public List<Models.Registration> GetByEnterPriseName(string EnterPriseName)
        {
            return db.Registrations.AsNoTracking().Where(x => x.EnterPriseName == EnterPriseName).ToList();
        }

        #region 删除
        /// <summary>
        /// 删除当前申请
        /// </summary>
        /// <param name="projId"></param>
        /// <returns></returns>
        public void Delete(Guid id)
        {
            Registration regis = db.Registrations.Find(id);
            if (regis != null)
            {
                db.Registrations.Remove(regis);
                db.SaveChanges();

            }
        }

     
        #endregion
    }
}
