using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseRegistration.Data.Models;

namespace EnterpriseRegistration.Data
{
    public interface ICustomMapService
    {
        List<CustomMap> GetAll();
        List<CustomMap> GetByMapCode(string mapcode);
        string GetMapCodeByCustomCode(string customcode);
    }
}
