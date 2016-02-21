using System.Windows.Controls;
using Microsoft.Practices.ServiceLocation;

namespace Warehouse.Wpf.UI.Modules.Products
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = ServiceLocator.Current.TryResolve<MainViewModel>();
        }
    }
}
