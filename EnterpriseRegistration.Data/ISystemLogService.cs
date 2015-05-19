using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Data
{
    public interface ISystemLogService
    {
        void Save(Models.SystemLog log);

        List<Models.SystemLog> GetAll();
        List<Models.SystemLog> GetByTypes(string types);
    }
}
