using Warehouse.Silverlight.SettingsModule.ViewModels;

namespace Warehouse.Silverlight.SettingsModule.Views
{
    public partial class SettingsView
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        public SettingsView(SettingsViewModel vm) : this()
        {
            DataContext = vm;
        }
    }
}
