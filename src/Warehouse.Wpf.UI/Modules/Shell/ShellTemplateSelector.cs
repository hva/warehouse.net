using System.Windows;
using System.Windows.Controls;
using Warehouse.Wpf.UI.Modules.Shell.LoggedIn;
using Warehouse.Wpf.UI.Modules.Shell.Login;

namespace Warehouse.Wpf.UI.Modules.Shell
{
    public class ShellTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LoginTemplate { get; set; }
        public DataTemplate LoggedInTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is LoginViewModel)
            {
                return LoginTemplate;
            }

            if (item is LoggedInViewModel)
            {
                return LoggedInTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
