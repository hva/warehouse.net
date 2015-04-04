using System.Diagnostics;
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
    }
}
