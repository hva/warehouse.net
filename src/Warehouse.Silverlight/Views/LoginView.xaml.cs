using Warehouse.Silverlight.ViewModels;

namespace Warehouse.Silverlight.Views
{
    public partial class LoginView
    {
        public LoginView()
        {
            InitializeComponent();
        }

        public LoginView(LoginViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
    }
}
