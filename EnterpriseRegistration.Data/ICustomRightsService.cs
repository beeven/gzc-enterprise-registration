using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseRegistration.Data.Models;
namespace EnterpriseRegistration.Data
{
    public interface ICustomRightsService
    {
        List<CustomRights> GetByCustomCode(string customCode);

        void UpdateCustomRights(string customCode, List<string> AvailableCustomCode);
    }
}
