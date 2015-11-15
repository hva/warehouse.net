using System.Windows.Controls;
using Microsoft.Practices.ServiceLocation;

namespace Warehouse.Wpf.UI.Modules.Settings
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();

            // TODO: replace woth ViewModelLocator.AutoWireViewModel="True"
            DataContext = ServiceLocator.Current.TryResolve<SettingsViewModel>();
        }
    }
}
