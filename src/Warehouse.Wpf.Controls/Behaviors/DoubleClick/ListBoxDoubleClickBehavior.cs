using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Warehouse.Wpf.Controls.Behaviors
{
    public class ListBoxDoubleClickBehavior : BehaviorBase<ListBox>
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

        protected override void OnLoaded()
        {
            AssociatedObject.MouseDoubleClick += OnDoubleClick;
        }

        protected override void OnUnloaded()
        {
            AssociatedObject.MouseDoubleClick -= OnDoubleClick;
        }

        private void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var obj = (DependencyObject)e.OriginalSource;

            while (obj != null && !AssociatedObject.Equals(obj))
            {
                if (obj.GetType() == typeof(ListBoxItem))
                {
                    var context = ((ListBoxItem)obj).DataContext;
                    if (Command != null && Command.CanExecute(context))
                    {
                        Command.Execute(context);
                    }

                    break;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
        }
    }
}
