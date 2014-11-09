using Warehouse.Silverlight.ViewModels;

namespace Warehouse.Silverlight.Views
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
