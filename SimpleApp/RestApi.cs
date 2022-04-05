using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleApp
{
    internal static class RestApi
    {
        const string URI = "http://localhost:8080";

        class DocListWrapper
        {
            public List<DocsRecord> Files = null;
        }

        class DocWrapper
        {
            public string File = null;
        }

        public class DocsRecord
        {
            public string name = null;
            public string path = null;
        }

        public static List<DocsRecord> GetDocs()
        {
            const string URL = "http://localhost:8080/api/dokumentumok";
            string response = "";
            using (var client = new WebClient() { UseDefaultCredentials = true })
            {
                response = client.DownloadString(URL);
            }
            var jsonObj = JsonConvert.DeserializeObject<DocListWrapper>(response);
            return jsonObj.Files;
        }

        public static void DownloadFile(string name, string downloadPath)
        {
            string URL = $"{URI}/api/dokumentumok/{name}";
            Program.Log("Download started");
            string response = null;
            using (var client = new WebClient() { UseDefaultCredentials = true })
            {
                response = client.DownloadString(URL);
            }

            try
            {
                var jsonObj = JsonConvert.DeserializeObject<DocWrapper>(response);
                var bytes = Convert.FromBase64String(jsonObj.File);
                string finalPath = Path.Combine(downloadPath, name);
                File.WriteAllBytes(finalPath, bytes);
                Program.Log($"Download finished: {finalPath}");
            }
            catch (Exception e2)
            {
                Program.Log($"parsing exception: {e2.Message}");
                throw;
            }
        }
    }
}
