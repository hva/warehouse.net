using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Warehouse.Silverlight.Controls.Behaviors
{
    public class ListBoxDoubleClickBehavior : DoubleClickBehavior<ListBoxItem>
    {
        #region Command

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(ListBoxDoubleClickBehavior),
            new PropertyMetadata(null)
        );

        #endregion


        protected override void OnDoubleClick(ListBoxItem element)
        {
            var param = element.DataContext;
            if (Command != null && Command.CanExecute(param))
            {
                Command.Execute(param);
            }
        }
    }
}
