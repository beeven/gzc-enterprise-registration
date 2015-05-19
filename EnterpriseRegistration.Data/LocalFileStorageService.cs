using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;

namespace EnterpriseRegistration.Data
{
    public class LocalFileStorageService: IFileStorageService
    {
        static String storePath;// = System.Configuration.ConfigurationManager.AppSettings["attachmentStore"];
        static  string connString;// = System.Configuration.ConfigurationManager.ConnectionStrings["EnterpriseRegistration"].ConnectionString;

        public LocalFileStorageService()
        {
            storePath = System.Configuration.ConfigurationManager.AppSettings["attachmentStore"]; 
            connString = System.Configuration.ConfigurationManager.ConnectionStrings["EnterpriseRegistration"].ConnectionString;

            if (storePath == null)
            {
                storePath = System.IO.Path.GetFullPath(".\attachments");
            }

            if (!Directory.Exists(storePath))
            {
                Directory.CreateDirectory(storePath);
            }
        }
        public Guid Upload(System.IO.Stream stream)
        {
            StoreInfo storeInfo = GenerateStoreInfo();
            var fs = File.OpenWrite(storeInfo.Path);
            stream.CopyTo(fs);
            fs.Close();
            stream.Flush();
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into FileTable values(@Id,@Path)", conn);
                cmd.Parameters.AddWithValue("@Id", storeInfo.Id);
                cmd.Parameters.AddWithValue("@Path", storeInfo.Path);
                
                cmd.ExecuteNonQuery();
            }

            return storeInfo.Id;
        }

        public void Download(Stream stream, Guid id)
        {
            String path = null;
            using(var conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select path from FileTable where id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                path = cmd.ExecuteScalar() as String;
            }
            if (path != null)
            {
                FileStream fs = File.OpenRead(path);
                fs.CopyTo(stream);
                fs.Close();
                stream.Flush();
            }
            else
            {
                throw new FileNotFoundException(String.Format("No file belongs to id {0}",id));
            }
        }


        public void Delete(Guid id)
        {
            String path = null;
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select path from FileTable where id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                path = cmd.ExecuteScalar() as String;
                if (path != null)
                {
                    File.Delete(path);
                    SqlCommand delCmd = new SqlCommand("delete from FileTable where id=@Id", conn);
                    delCmd.Parameters.AddWithValue("@Id", id);
                    delCmd.ExecuteNonQuery();
                }
                
            }
        }


        class StoreInfo
        {
            public Guid Id {get;set;}
            public String Path { get; set; }
        }

        private StoreInfo GenerateStoreInfo()
        {
            StoreInfo info = new StoreInfo();
            info.Id = Guid.NewGuid();
            info.Path = Path.Combine(storePath, info.Id.ToString("D"));
            return info;
        }



    }

}
