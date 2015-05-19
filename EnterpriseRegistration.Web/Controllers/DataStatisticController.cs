using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using EnterpriseRegistration.Data;
using EnterpriseRegistration.Data.Models;
using EnterpriseRegistration.Web.Models;
namespace EnterpriseRegistration.Web.Controllers
{
    public class DataStatisticController : Controller
    {
        private static DataTable dt;
        private static DataTable dtExcel;
        private List<Customs> customs;
        //
        // GET: /DataStatistic/
        public DataStatisticController()
        {
            ICustomsService customService = new CustomService();
            customs = customService.GetAll();
        }

        public ActionResult Index()
        {
            ICustomsService cusService = new CustomService();
            List<Customs> models = cusService.GetAll();
            return View(models.AsEnumerable());
        }

        public ActionResult Query(int pagesize, int currentpage,string filter,string time)
        {
            dt = new DataTable();
            dt.Columns.Add("申请企业名称", Type.GetType("System.String"));
            dt.Columns.Add("组织机构代码", Type.GetType("System.String"));
            dt.Columns.Add("申请企业类别", Type.GetType("System.String"));
            dt.Columns.Add("申请时间", Type.GetType("System.String"));
            dt.Columns.Add("申请企业联系人", Type.GetType("System.String"));
            dt.Columns.Add("联系电话", Type.GetType("System.String"));
            dt.Columns.Add("申请备案现场海关", Type.GetType("System.String"));
            dt.Columns.Add("申请业务模式", Type.GetType("System.String"));
            dt.Columns.Add("企业备案号生成时间", Type.GetType("System.String"));
            dt.Columns.Add("企业备案号", Type.GetType("System.String"));
            dt.Columns.Add("各环节作业办结时间", Type.GetType("System.String"));
            

            IRegistrationService registrationService = new RegistrationService();
            IRegInfoService regInfoservice = new MSSQLRegInfoService();
            List<Registration> registrations = registrationService.GetAll().Where(x => x.EnterPriseName.Contains(filter)||x.OrgCode.Contains(filter)).ToList();

            List<Registration> newItems = new List<Registration>();
            for (int i = pagesize * (currentpage-1); i < pagesize * currentpage; i++)
            {
                if (i >= registrations.Count())
                {
                    break;
                }
                newItems.Add(registrations[i]);
            }
            DataStatisticQuery data = new DataStatisticQuery();
            data.ListCount = registrations.Count;
            data.TotalPage = data.ListCount / pagesize+(data.ListCount%pagesize==0?0:1);
            foreach (Registration item in newItems)
            {
                RegInfo reginfo = regInfoservice.Get(item.AttchID);
                DataRow dr = dt.NewRow();

                dr[0] = item.EnterPriseName;

                string EnterpriseCategory = "";
                if (item.ManagementFlag == 0)
                    EnterpriseCategory += "跨境贸易电子商务经营企业,";
                if (item.TransactionFlag == 0)
                    EnterpriseCategory += "跨境贸易电子商务交易企业,";
                if (item.LogisticsFlag == 0)

                    EnterpriseCategory += "跨境贸易电子商务物流企业,";
                if (item.PayFlag == 0)
                    EnterpriseCategory += "跨境贸易电子商务支付企业,";
                EnterpriseCategory = EnterpriseCategory.TrimEnd(',');
                dr[2] = EnterpriseCategory;
                dr[3] = reginfo.ReceivedDate.ToString("yyyy-MM-dd HH:mm");
                dr[4] = item.EnterPriseContact;
                dr[5] = item.EnterPrisePhone;

                string customName = item.CustomsName;
                foreach (Customs cus in customs)
                {
                    if (cus.CustomName == customName)
                    {
                        customName += "(" + cus.CustomCode + ")";
                        break;
                    }
                }
                dr[6] = customName;

                string businessMode = "";
                if (item.OnlineFlag == 0)
                    businessMode += "网购保税进口,";
                if (item.DirectFlag == 0)
                    businessMode += "直购进口,";
                businessMode = businessMode.TrimEnd(',');
                dr[7] = businessMode;
                dr[8] = reginfo.GetEntAccessDate == null ? "暂未生成" : DateTime.Parse(reginfo.GetEntAccessDate.ToString()).ToString("yyyy-MM-dd HH:mm");
                if (reginfo.isRecordPass == 1)
                    dr[9] = reginfo.RecordNum;
                else
                    dr[9] = "未通过审核";
                dr[10] =reginfo.RepliedDate==null?"未回复": DateTime.Parse(reginfo.RepliedDate.ToString()).ToString("yyyy-MM-dd HH:mm");
                dr[1] = item.OrgCode;
                dt.Rows.Add(dr);
                
            }
            #region 填充下载的表格
            if (currentpage == 1)
            {
                dtExcel = new DataTable();
                dtExcel.Columns.Add("申请企业名称", Type.GetType("System.String"));
                dtExcel.Columns.Add("组织机构代码", Type.GetType("System.String"));
                dtExcel.Columns.Add("申请企业类别", Type.GetType("System.String"));
                dtExcel.Columns.Add("申请时间", Type.GetType("System.String"));
                dtExcel.Columns.Add("申请企业联系人", Type.GetType("System.String"));
                dtExcel.Columns.Add("联系电话", Type.GetType("System.String"));
                dtExcel.Columns.Add("申请备案现场海关", Type.GetType("System.String"));
                dtExcel.Columns.Add("申请业务模式", Type.GetType("System.String"));
                dtExcel.Columns.Add("企业备案号生成时间", Type.GetType("System.String"));
                dtExcel.Columns.Add("企业备案号", Type.GetType("System.String"));
                dtExcel.Columns.Add("各环节作业办结时间", Type.GetType("System.String"));

                foreach (Registration item in registrations)
                {
                    RegInfo reginfo = regInfoservice.Get(item.AttchID);
                    DataRow dr = dtExcel.NewRow();

                    dr[0] = item.EnterPriseName;

                    string EnterpriseCategory = "";
                    if (item.ManagementFlag == 0)
                        EnterpriseCategory += "跨境贸易电子商务经营企业,";
                    if (item.TransactionFlag == 0)
                        EnterpriseCategory += "跨境贸易电子商务交易企业,";
                    if (item.LogisticsFlag == 0)
                        EnterpriseCategory += "跨境贸易电子商务物流企业,";
                    if (item.PayFlag == 0)
                        EnterpriseCategory += "跨境贸易电子商务支付企业,";
                    EnterpriseCategory = EnterpriseCategory.TrimEnd(',');
                    dr[2] = EnterpriseCategory;
                    dr[3] = reginfo.ReceivedDate.ToString("yyyy-MM-dd HH:mm");
                    dr[4] = item.EnterPriseContact;
                    dr[5] = item.EnterPrisePhone;

                    string customName = item.CustomsName;
                    foreach (Customs cus in customs)
                    {
                        if (cus.CustomName == customName)
                        {
                            customName += "(" + cus.CustomCode + ")";
                            break;
                        }
                    }
                    dr[6] = customName;

                    string businessMode = "";
                    if (item.OnlineFlag == 0)
                        businessMode += "网购保税进口,";
                    if (item.DirectFlag == 0)
                        businessMode += "直购进口,";
                    businessMode = businessMode.TrimEnd(',');
                    dr[7] = businessMode;
                    dr[8] = reginfo.GetEntAccessDate == null ? "暂未生成" : DateTime.Parse(reginfo.GetEntAccessDate.ToString()).ToString("yyyy-MM-dd HH:mm");
                    if (reginfo.isRecordPass == 1)
                        dr[9] = reginfo.RecordNum;
                    else
                        dr[9] = "未通过审核";
                    dr[10] = reginfo.RepliedDate == null ? "未回复" : DateTime.Parse(reginfo.RepliedDate.ToString()).ToString("yyyy-MM-dd HH:mm");
                    dr[1] = item.OrgCode;
                    dtExcel.Rows.Add(dr);

                }
            }
            #endregion

            data.dtItem = dt;
            return PartialView("StatisticList", data);
        }

        public void Excel()
        {
            ExcelHelper excel = new ExcelHelper();
            
            string displayName = "备案审核系统数据统计" + DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + ".xls";
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(displayName));
            excel.DataTableToExcel(Response.OutputStream, dtExcel, displayName, "数据统计", true);
            
            Response.Flush();
            Response.End();
        }


    }
}
