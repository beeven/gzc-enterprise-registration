using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Data
{
    public class PDFHelper
    {
        #region 读取pdf
        /// <summary>
        /// 获取pdf表单值填充到Dictionary
        /// </summary>
        /// <param name="pdfPath"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ReadForm(byte[] context)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            PdfReader pdfRead = null;
            object values;
            try
            {
                pdfRead = new PdfReader(context);
                AcroFields pdfFormFields = pdfRead.AcroFields;
                foreach (var de in pdfFormFields.Fields)
                {
                    string value = pdfFormFields.GetField(de.Key);
                    //不填的文本框或多选框值为空，需转换对应类型值
                    switch (de.Key)
                    {
                        case "OnlineFlag":
                            values = value == "是" ? 0 : 1;
                            break;
                        case "DirectFlag":
                            values = value == "是" ? 0 : 1;
                            break;
                        case "ManagementFlag":
                            values = value == "是" ? 0 : 1;
                            break;
                        case "TransactionFlag":
                            values = value == "是" ? 0 : 1;
                            break;
                        case "LogisticsFlag":
                            values = value == "是" ? 0 : 1;
                            break;
                        case "PayFlag":
                            values = value == "是" ? 0 : 1;
                            break;
                        case "Abroad":
                            values = value == "是" ? 0 : 1;
                            break;
                        case "LegalCopyFlag":
                            values = value == "是" ? 0 : 1;
                            break;
                        case "TaxCopyFlag":
                            values = value == "是" ? 0 : 1;
                            break;
                        case "OrgCopyFlag":
                            values = value == "是" ? 0 : 1;
                            break;
                        case "PayCopyFlag":
                            values = value == "是" ? 0 : 1;
                            break;
                        case "LicenseCopyFlag":
                            values = value == "是" ? 0 : 1;
                            break;
                        case "UnitsFlag":
                            values = value == "是" ? 0 : 1;
                            break;
                        case "OperateFlag":
                            values = value == "是" ? 0 : 1;
                            break;
                        default:
                            values = value;
                            break;
                    }

                    dic.Add(de.Key, values);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return dic;
        }

        #endregion
    }
}
