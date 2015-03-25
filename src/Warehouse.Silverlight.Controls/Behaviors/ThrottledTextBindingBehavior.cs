using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Threading;

namespace Warehouse.Silverlight.Controls.Behaviors
{
    public class ThrottledTextBindingBehavior : Behavior<TextBox>
    {
        private readonly DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(400) };

        protected BindingExpression expression;

        protected sealed override void OnAttached()
        {
            base.OnAttached();

            expression = AssociatedObject.GetBindingExpression(TextBox.TextProperty);
            AssociatedObject.TextChanged += OnTextChanged;
            AssociatedObject.Loaded += OnLoaded;
        }

        protected sealed override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.TextChanged -= OnTextChanged;
            AssociatedObject.Loaded -= OnLoaded;
        }

        protected bool IsTextChangingLocked
        {
            set
            {
                if (value)
                {
                    AssociatedObject.TextChanged -= OnTextChanged;
                }
                else
                {
                    AssociatedObject.TextChanged += OnTextChanged;
                }
            }
        }

        protected virtual void OnLoaded()
        {
        }

        protected virtual void UpdateSource()
        {
            expression.UpdateSource();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            OnLoaded();
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

            UpdateSource();
        }
    }
}
