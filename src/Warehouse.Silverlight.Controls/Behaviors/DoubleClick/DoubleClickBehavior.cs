using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Warehouse.Silverlight.Controls.Behaviors
{
    public abstract class DoubleClickBehavior<T> : Behavior<UIElement> where T : UIElement
    {
        private DateTime lastTime;
        private T element;
        private const int delay = 300;

        protected abstract void OnDoubleClick(T element);

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonUp += MouseLeftButtonUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseLeftButtonUp -= MouseLeftButtonUp;
        }

        private void MouseLeftButtonUp(object s, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(null);
            var elementsUnderMouse = System.Windows.Media.VisualTreeHelper.FindElementsInHostCoordinates(pos, AssociatedObject);
            var currentElement = elementsUnderMouse.OfType<T>().FirstOrDefault();

            if ((DateTime.Now.Subtract(lastTime).TotalMilliseconds) < delay && element != null && element.Equals(currentElement))
            {
                OnDoubleClick(element);
            }
            lastTime = DateTime.Now;
            element = currentElement;
        }
    }
}
