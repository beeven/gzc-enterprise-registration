using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseRegistration.Data.Models;

namespace EnterpriseRegistration.Data
{
    public interface ICustomsService
    {
        List<Customs> GetAll();
        void Update(string id, string contact, string tel);
    }
}
