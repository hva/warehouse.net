using NCron.Fluent.Crontab;
using NCron.Fluent.Generics;
using NCron.Service;

namespace Warehouse.Utils.Backup
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrap.Init(args, ServiceSetup);
        }

        private static void ServiceSetup(ISchedulingService service)
        {
            service.At("*/3 * * * *").Run<Job>();
        }
    }
}
