using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Warehouse.Wpf.Events.Navigation;
using Warehouse.Wpf.Infrastructure.Interfaces;
using Warehouse.Wpf.Module.ProductDetail.Create;
using Warehouse.Wpf.Module.ProductDetail.Edit;

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
            eventAggregator.GetEvent<NavigateProductCreateEvent>().Subscribe(OnNavigateProductCreate, true);
            eventAggregator.GetEvent<NavigateProductEditEvent>().Subscribe(OnNavigateProductEdit, true);
        }

        public void OnNavigateProductCreate(object obj)
        {
            var window = new ProductCreateWindow { Owner = mainWindow };
            ((INavigationAware)window.DataContext).OnNavigatedTo();
            window.ShowDialog();
        }

        private void OnNavigateProductEdit(object obj)
        {
            var window = new ProductEditWindow { Owner = mainWindow };
            window.ShowDialog();
        }
    }
}
