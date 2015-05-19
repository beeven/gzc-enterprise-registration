using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Data
{
    public interface IRegistrationService
    {
        //分析Excel
        DataTable AnalysisExcel(Stream RegistrationStream, string fileName);

        //分析pdf
        Dictionary<string, object> AnalysisPDF(byte[] Context);

        //将数据导入到数据库
        void InsertExcel(DataTable dt,int AttchID);

        void InsertPDF(Dictionary<string, object> dict, int AttchID);

        List<Models.Registration> GetAll();

        Models.Registration Get(Guid id);

        Models.Registration GetByAttchID(int AttchID);
        //根据企业名称查找当前企业备案信息
        List<Models.Registration> GetByEnterPriseName(string EnterPriseName);

        void Delete(Guid id);

    }
}
