using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Unity;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Module.ProductDetail.ViewModels;
using Warehouse.Wpf.Module.ProductDetail.Views;

namespace Warehouse.Wpf.Module.ProductDetail
{
    public class ProductDetailModule : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public void Initialize()
        {
            Container.RegisterType<object, CreateProductView>(Consts.CreateProductView);
            ViewModelLocationProvider.Register(typeof(CreateProductView).FullName, () => Container.Resolve<CreateProductViewModel>());
        }
    }
}
