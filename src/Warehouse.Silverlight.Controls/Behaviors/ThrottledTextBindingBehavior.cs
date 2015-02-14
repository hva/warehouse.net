using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Threading;

namespace Warehouse.Silverlight.Controls.Behaviors
{
    public class ThrottledTextBindingBehavior : Behavior<TextBox>
    {
        private BindingExpression expression;
        private DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(400) };

        protected override void OnAttached()
        {
            base.OnAttached();

            expression = AssociatedObject.GetBindingExpression(TextBox.TextProperty);
            AssociatedObject.TextChanged += OnTextChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.TextChanged += OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
                timer.Tick -= Tick;
            }
            timer.Tick += Tick;
            timer.Start();
        }

        private void Tick(object sender, EventArgs e)
        {
            timer.Stop();
            timer.Tick -= Tick;

            if (expression != null)
            {
                expression.UpdateSource();
            }
        }
    }
}
