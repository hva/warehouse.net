using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NLog;

namespace Warehouse.Utils.Backup
{
    class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            if (!TryDump()) return;

            var fileName = string.Format("skill_{0:yyyyMMdd_HHmm}.zip", DateTime.Now);
            if (!TryZip(fileName)) return;

            var token = LoadToken();
            if (token == null) return;

            var task = GetUploadLink(fileName, token);
            task.Wait();

            var link = task.Result;
            if (link == null) return;

            var task2 = UploadFile(fileName, link);
            task2.Wait();
        }

        private static bool TryDump()
        {
            var info = new ProcessStartInfo
            {
                FileName = "mongodump",
                Arguments = "--db skill",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
            };
            var process = new Process { StartInfo = info };
            process.Start();
            process.WaitForExit();

            var error = process.StandardError.ReadToEnd();

            if (string.IsNullOrEmpty(error))
            {
                return true;
            }
            logger.Error(error);
            return false;
        }

        private static bool TryZip(string fileName)
        {
            try
            {
                ZipFile.CreateFromDirectory("dump", fileName);
                return true;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
            return false;
        }

        private static string LoadToken()
        {
            try
            {
                using (var reader = new StreamReader("token.txt"))
                {
                    return reader.ReadLine();
                }
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
            }
            return null;
        }

        private async static Task<string> GetUploadLink(string fileName, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", token);
                client.BaseAddress = new Uri("https://cloud-api.yandex.net:443");

                var path = string.Concat("skill-backup/", fileName);
                var linkUriString = string.Format("/v1/disk/resources/upload?path={0}", WebUtility.UrlEncode(path));

                var resp = await client.GetAsync(linkUriString);
                var content = await resp.Content.ReadAsStringAsync();
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    var json = JObject.Parse(content);
                    return json["href"].ToString();
                }
                logger.Error(content);
                return null;
            }
        }

        private static async Task UploadFile(string fileName, string link)
        {
            using (var client = new HttpClient())
            using (var stream = File.OpenRead(fileName))
            using (var content = new StreamContent(stream))
            {
                var resp = await client.PutAsync(link, content);
                if (resp.StatusCode == HttpStatusCode.Created)
                {
                    logger.Trace("upload succeed");
                }
                else
                {
                    var errorContent = await resp.Content.ReadAsStringAsync();
                    logger.Error(errorContent);
                }
            }
        }
    }
}
