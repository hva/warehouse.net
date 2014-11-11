using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Warehouse.Silverlight.Controls.Behaviors
{
    public class EnterPressedBehavior : Behavior<FrameworkElement>
    {
        #region Command

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(EnterPressedBehavior), new PropertyMetadata(null));

        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.KeyUp += OnKeyUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.KeyUp -= OnKeyUp;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateBinding();
                TryToExecuteCommand();
            }
        }

        private void UpdateBinding()
        {
            DependencyProperty prop = null;
            if (AssociatedObject is TextBox)
            {
                prop = TextBox.TextProperty;
            }
            else if (AssociatedObject is PasswordBox)
            {
                prop = PasswordBox.PasswordProperty;
            }

            if (prop != null)
            {
                var expr = AssociatedObject.GetBindingExpression(prop);
                if (expr != null)
                {
                    expr.UpdateSource();
                }
            }
        }

        private void TryToExecuteCommand()
        {
            if (Command == null) return;

            if (Command.CanExecute(null))
            {
                Command.Execute(null);
            }
        }
    }
}
