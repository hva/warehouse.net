using Warehouse.Silverlight.NavigationModule.ViewModels;

namespace Warehouse.Silverlight.NavigationModule.Views
{
    public partial class TopMenu
    {
        public TopMenu()
        {
            InitializeComponent();
        }

        public TopMenu(TopMenuViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
    }
}
