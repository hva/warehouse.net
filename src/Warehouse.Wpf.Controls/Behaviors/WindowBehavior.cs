using System.Windows;
using System.Windows.Interactivity;

namespace Warehouse.Wpf.Controls.Behaviors
{
    public class WindowBehavior : Behavior<Window>
    {
        public bool IsWindowOpen
        {
            get { return (bool)GetValue(IsWindowOpenProperty); }
            set { SetValue(IsWindowOpenProperty, value); }
        }

        public static readonly DependencyProperty IsWindowOpenProperty =
            DependencyProperty.Register("IsWindowOpen", typeof(bool), typeof(WindowBehavior), new PropertyMetadata(false, OnIsWindowOpenChanged));

        private static void OnIsWindowOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WindowBehavior)d).UpdateIsWindowOpen();
        }

        private void UpdateIsWindowOpen()
        {
            // is used only for closing
            if (!IsWindowOpen)
            {
                AssociatedObject.Close();
            }
        }
    }
}
