using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Unity;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Module.ProductDetail.Create;

namespace Warehouse.Wpf.Module.ProductDetail
{
    public class ProductDetailModule : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public void Initialize()
        {
            Container.RegisterType<object, ProductCreateView>(Consts.CreateProductView);
            ViewModelLocationProvider.Register(typeof(ProductCreateView).FullName, () => Container.Resolve<ProductCreateViewModel>());
        }
    }
}
