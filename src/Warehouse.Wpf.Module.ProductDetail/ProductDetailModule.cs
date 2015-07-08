using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Warehouse.Wpf.Infrastructure.Events;
using Warehouse.Wpf.Module.ProductDetail.Create;

namespace Warehouse.Wpf.Module.ProductDetail
{
    public class ProductDetailModule : IModule
    {
        private readonly IEventAggregator eventAggregator;
        private readonly Window mainWindow;

        public ProductDetailModule(IEventAggregator eventAggregator, Window mainWindow)
        {
            this.eventAggregator = eventAggregator;
            this.mainWindow = mainWindow;
        }

        public void Initialize()
        {
            eventAggregator.GetEvent<ProductCreateRequestEvent>().Subscribe(OnProductCreateRequest, true);
        }

        public void OnProductCreateRequest(object obj)
        {
            var window = new ProductCreateWindow { Owner = mainWindow };
            window.ShowDialog();
        }
    }
}
