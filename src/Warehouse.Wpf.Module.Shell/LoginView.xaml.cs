using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Mvvm;

namespace Warehouse.Wpf.Module.Shell
{
    public partial class LoginView : UserControl, IView
    {
        public LoginView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(this, login);
        }
    }
}
