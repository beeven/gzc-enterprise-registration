using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Data
{
    public interface IFileStorageService
    {
        Guid Upload(Stream data);
        void Download(Stream outStream, Guid id);

        void Delete(Guid id);
    }
}
