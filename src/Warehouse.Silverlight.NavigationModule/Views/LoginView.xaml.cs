using Warehouse.Silverlight.NavigationModule.ViewModels;

namespace Warehouse.Silverlight.NavigationModule.Views
{
    public partial class LoginView
    {
        public LoginView()
        {
            InitializeComponent();

            if (DataContext == null)
            {
                DataContext = new LoginViewModel();
            }
        }
    }
}
