using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Receipt.Logging
{
    public interface ILogger
    {
        void Log(String content);
    }
}
