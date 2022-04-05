using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace SimpleService
{
    public struct DataStruct
    {
        public DataStruct(string name, string path)
        {
            this.name = name;
            this.path = path;
        }

        public string name;
        public string path;
    }

    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "/api/dokumentumok")]
        [return: MessageParameter(Name = "Files")]
        DataStruct[] GetDocsContent();

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "/api/dokumentumok/{name}")]
        [return: MessageParameter(Name = "File")]
        string GetDoc(string name);
    }

    public class Service : IService
    {
        public string SayHello(string name)
        {
            Console.WriteLine("From SayHello");
            return "From SayHello " + name;
        }

        public DataStruct[] GetDocsContent()
        {
            string targetPath = GetPath();
            string[] files = System.IO.Directory.GetFiles(targetPath);
            List<DataStruct> results = new List<DataStruct>();
            foreach (string s in files)
            {
                System.IO.FileInfo fi = null;
                try
                {
                    fi = new System.IO.FileInfo(s);
                }
                catch (System.IO.FileNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                //Console.WriteLine("{0} : {1}", fi.Name, fi.Directory);
                results.Add(new DataStruct(fi.Name, fi.Directory.Name));
            }

            return results.ToArray();
        }


        public string GetDoc(string name)
        {
            string path = GetPath();
            if (path == null)
            {
                Console.WriteLine("Doc not found");
                return null;
            }
            path = $"{path}/{name}";
            if (!File.Exists(path))
            {
                Console.WriteLine("file does not exist");
                return null;
            }

            Byte[] bytes = null;
            try
            {
                bytes = File.ReadAllBytes(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            return Convert.ToBase64String(bytes);
        }

        public string GetPath()
        {
            string targetPath = System.IO.Directory.GetCurrentDirectory();
            string configPath = ConfigurationManager.AppSettings["docsPath"];
            if (!string.IsNullOrEmpty(configPath))
            {
                if (configPath[0] == '/' || configPath[0] == '\\' || configPath[0] == '.')
                {
                    string trimmed = configPath.TrimStart('\\', '/', '.');
                    targetPath = Path.Combine(targetPath, trimmed);
                }
                else
                {
                    targetPath = configPath;
                }
            }
            else
            {
                Console.WriteLine("config path is empty");
            }
            Console.WriteLine($"target path: {targetPath}");

            if (!System.IO.Directory.Exists(targetPath))
            {
                Console.Write($"Target path doesn't exist {targetPath}");
                return null;
            }

            return targetPath;
        }
    }
}