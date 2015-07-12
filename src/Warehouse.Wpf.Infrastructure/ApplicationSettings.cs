using Warehouse.Wpf.Infrastructure.Interfaces;

namespace Warehouse.Wpf.Infrastructure
{
    public class ApplicationSettings : IApplicationSettings
    {
        public string Endpoint { get { return "http://localhost:63270/"; } }
    }
}
