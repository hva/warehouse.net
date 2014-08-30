using System.Windows;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

namespace Warehouse.Silverlight
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            Application.Current.RootVisual = (UIElement)Shell;
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
        }
    }
}
