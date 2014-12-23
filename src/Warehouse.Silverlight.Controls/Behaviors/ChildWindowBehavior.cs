using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Warehouse.Silverlight.Controls.Behaviors
{
    public class ChildWindowBehavior : Behavior<ChildWindow>
    {
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
            if (e.Key == Key.Escape)
            {
                AssociatedObject.Close();
            }
        }
    }
}
