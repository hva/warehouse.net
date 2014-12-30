namespace Warehouse.Silverlight.SettingsModule
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
