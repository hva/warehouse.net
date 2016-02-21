using System.Windows.Controls;
using Microsoft.Practices.ServiceLocation;

namespace Warehouse.Wpf.UI.Modules.Files
{
    public partial class FilesView : UserControl
    {
        public FilesView()
        {
            InitializeComponent();
            DataContext = ServiceLocator.Current.TryResolve<FilesViewModel>();
        }
    }
}
