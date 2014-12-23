using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Warehouse.Silverlight.Controls.Behaviors
{
    public class DataGridRowDoubleClickBehavior : Behavior<DataGrid>
    {
        private DateTime lastTime;
        private DataGridRow row;
        private const int delay = 300;

        #region Command

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(DataGridRowDoubleClickBehavior),
            new PropertyMetadata(null)
        );

        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.LoadingRow += LoadingRow;
            AssociatedObject.UnloadingRow += UnloadingRow;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.LoadingRow -= LoadingRow;
            AssociatedObject.UnloadingRow -= UnloadingRow;
        }

        private void LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.MouseLeftButtonUp += MouseLeftButtonUp;
        }

        private void UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.MouseLeftButtonUp -= MouseLeftButtonUp;
        }

        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((DateTime.Now.Subtract(lastTime).TotalMilliseconds) < delay && row != null && row.Equals(sender))
            {
                ExecuteCommand();
            }
            lastTime = DateTime.Now;
            row = sender as DataGridRow;
        }

        private void ExecuteCommand()
        {
            if (Command == null) return;

            var param = row.DataContext;
            if (Command.CanExecute(param))
            {
                Command.Execute(param);
            }
        }
    }
}
