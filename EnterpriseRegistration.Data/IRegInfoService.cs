using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Data
{
    public interface IRegInfoService
    {
        int Save(Models.RegInfo info);
        List<Models.RegInfo> GetAll();
        List<Models.RegInfo> GetAllByUserCustomCode(List<string> roleIds, string customCode);
        Models.RegInfo Get(int id);

        void Update(Models.RegInfo info);

        void SetReplied(int id, string content);

        void Delete(int id);
    }
}
