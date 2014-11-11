using System.Windows;
using Warehouse.Silverlight.NavigationModule.ViewModels;

namespace Warehouse.Silverlight.NavigationModule.Views
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

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Browser.HtmlPage.Plugin.Focus();
            login.Focus();
            login.SelectAll();
        }
    }
}
