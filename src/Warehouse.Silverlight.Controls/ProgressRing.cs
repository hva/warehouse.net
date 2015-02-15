using System.Windows;
using System.Windows.Controls;

namespace Warehouse.Silverlight.Controls
{
    public class ProgressRing : Control
    {
        public ProgressRing()
        {
            DefaultStyleKey = typeof (ProgressRing);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateStates();
        }

        #region IsActive

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(ProgressRing), new PropertyMetadata(false, OnIsActiveChanged));

        private static void OnIsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ProgressRing)d).UpdateStates();
        }

        #endregion

        private void UpdateStates()
        {
            var state = IsActive ? "Active" : "Inactive";
            VisualStateManager.GoToState(this, state, false);
        }
    }
}
