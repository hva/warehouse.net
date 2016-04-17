using System.Configuration;
using NCron.Fluent.Crontab;
using NCron.Fluent.Generics;
using NCron.Service;
using System.Diagnostics;

namespace Warehouse.Utils.Backup
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Debugger.IsAttached)
            {
                var job = new Job();
                job.Execute();
            }
            else
            {
                Bootstrap.Init(args, ServiceSetup);
            }
        }

        private static void ServiceSetup(ISchedulingService service)
        {
            var crontab = ConfigurationManager.AppSettings["crontab"];
            service
                .At(crontab)
                .Run<Job>()
                .Named("mongo");
        }
    }
}
