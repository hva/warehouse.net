using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Warehouse.Wpf.Module.Settings
{
    public partial class ChangePasswordView : UserControl
    {
        public ChangePasswordView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(this, old);
        }

        private void ChangeButtonClick(object sender, RoutedEventArgs e)
        {
            var context = DataContext as ChangePasswordViewModel;
            if (context != null)
            {
                var args = new ChangePasswordArgs(old, @new, new2);
                if (context.SaveCommand != null && context.SaveCommand.CanExecute(args))
                {
                    context.SaveCommand.Execute(args);
                }
            }
        }
    }
}
