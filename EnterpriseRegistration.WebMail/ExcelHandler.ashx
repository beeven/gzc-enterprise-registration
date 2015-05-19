<%@ WebHandler Language="C#" Class="ExcelHandler" %>

using System;
using System.Web;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Text;

public class ExcelHandler : IHttpHandler
{

    private IWorkbook workbook = null;
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string fileName = HttpContext.Current.Server.MapPath("contents/广州海关跨境贸易电子商务备案支付企业名单.xls");
        //string fileName = "~/广州海关跨境贸易电子商务备案支付企业名单.xls";
        string sheetName = "支付企业";
        ISheet sheet = null;
        DataTable data = new DataTable();
        int startRow = 0;

        Stream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        if (fileName.IndexOf(".xlsx") > 0) // 2007版本
            workbook = new XSSFWorkbook(fs);
        else if (fileName.IndexOf(".xls") > 0) // 2003版本
            workbook = new HSSFWorkbook(fs);

        if (sheetName != null)
        {
            sheet = workbook.GetSheet(sheetName);
        }
        else
        {
            sheet = workbook.GetSheetAt(0);
        }
        if (sheet != null)
        {
            IRow firstRow = sheet.GetRow(0);
            int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

            if (true)
            {
                for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                {
                    DataColumn column = new DataColumn(firstRow.GetCell(i).StringCellValue);
                    data.Columns.Add(column);
                }
                startRow = sheet.FirstRowNum + 1;
            }
            else
            {
                startRow = sheet.FirstRowNum;
            }

            //最后一列的标号
            int rowCount = sheet.PhysicalNumberOfRows;
            for (int i = startRow; i <= rowCount; ++i)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue; //没有数据的行默认是null　　　　　　　

                DataRow dataRow = data.NewRow();
                for (int j = row.FirstCellNum; j < cellCount; ++j)
                {
                    dataRow[j] = row.GetCell(j) == null ? string.Empty : row.GetCell(j).ToString();
                }
                data.Rows.Add(dataRow);
            }
        }
        IList<ExcelModel> list = DatatableToMedel(data);
        //循环将DataTable转换成实体
        string json = JSON<ExcelModel>(list);
        //return data;
        context.Response.Write(json);
    }

    /// <summary>
    /// 转换List<T>的数据为JSON格式
    /// </summary>
    /// <typeparam name="T">类</typeparam>
    /// <param name="vals">列表值</param>
    /// <returns>JSON格式数据</returns>
    public static string JSON<T>(IList<ExcelModel> vals)
    {
        System.Text.StringBuilder st = new System.Text.StringBuilder();
        string result = string.Empty;
        try
        {
            System.Runtime.Serialization.Json.DataContractJsonSerializer s = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));

            foreach (ExcelModel city in vals)
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    s.WriteObject(ms, city);
                    st.Append(System.Text.Encoding.UTF8.GetString(ms.ToArray())+",");
                }
            }

            result = st.ToString().TrimEnd(',');
            result = "[" + result + "]";
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return result;
    }

    public static IList<ExcelModel> DatatableToMedel(DataTable dt)
    {
        IList<ExcelModel> list = new List<ExcelModel>();
        foreach (DataRow item in dt.Rows)
        {
            ExcelModel model = new ExcelModel();
            model.Id = int.Parse(item[0].ToString());
            model.EnterPriseName = item[1].ToString();
            model.Contact = item[2].ToString();
            model.Phone = item[3].ToString();
            model.Email = item[4].ToString();
            model.Progress = item[5].ToString();
            list.Add(model);
        }
        return list;
    }
    
    public class ExcelModel
    {
        public int Id { get; set; }
        public string EnterPriseName { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Progress { get; set; }
    }
    
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}