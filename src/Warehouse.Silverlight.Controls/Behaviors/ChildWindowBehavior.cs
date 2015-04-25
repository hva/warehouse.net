using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Warehouse.Silverlight.Controls.Behaviors
{
    public class ChildWindowBehavior : Behavior<ChildWindow>
    {
        public bool IsWindowOpen
        {
            get { return (bool)GetValue(IsWindowOpenProperty); }
            set { SetValue(IsWindowOpenProperty, value); }
        }

        public static readonly DependencyProperty IsWindowOpenProperty =
            DependencyProperty.Register("IsWindowOpen", typeof(bool), typeof(ChildWindowBehavior), new PropertyMetadata(true, OnIsWindowOpenChanged));

        private static void OnIsWindowOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ChildWindowBehavior)d).UpdateIsWindowsOpen((bool)e.NewValue);
        }


        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.KeyUp -= OnKeyUp;
            AssociatedObject.KeyUp += OnKeyUp;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                AssociatedObject.Close();
            }
        }

        private void UpdateIsWindowsOpen(bool isOpen)
        {
            if (!isOpen)
            {
                AssociatedObject.Close();
            }
        }
    }
}
