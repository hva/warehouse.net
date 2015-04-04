using System;
using System.Diagnostics;
using System.IO.Compression;
using NLog;

namespace Warehouse.Utils.Backup
{
    class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            if (TryDump())
            {
                TryZip();
            }
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
                logger.Trace("dump ok");
                return true;
            }
            logger.Error(error);
            return false;
        }

        private static bool TryZip()
        {
            try
            {
                var fileName = string.Format("skill_{0:yyyyMMdd_HHmm}.zip", DateTime.Now);
                ZipFile.CreateFromDirectory("dump", fileName);
                logger.Trace("zip ok");
                return true;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
            return false;
        }
    }
}
