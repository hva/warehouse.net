using System.Windows;
using System.Windows.Interactivity;

namespace Warehouse.Wpf.Controls.Behaviors
{
    public abstract class BehaviorBase<T> : Behavior<T> where T : FrameworkElement
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.Unloaded += OnUnloaded;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.Unloaded -= OnUnloaded;
            base.OnDetaching();
        }

        protected virtual void OnLoaded()
        {
        }

        protected virtual void OnUnloaded()
        {
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            OnLoaded();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            OnUnloaded();
        }
    }
}
