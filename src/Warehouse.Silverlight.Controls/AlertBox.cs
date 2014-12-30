using System.Windows;
using System.Windows.Controls;

namespace Warehouse.Silverlight.Controls
{
    [TemplateVisualState(Name = AlertState, GroupName = AlertStates)]
    [TemplateVisualState(Name = SuccessState, GroupName = AlertStates)]
    public class AlertBox : ContentControl
    {
        private const string AlertStates = "AlertStates";
        private const string AlertState = "Alert";
        private const string SuccessState = "Success";

        public AlertBox()
        {
            DefaultStyleKey = typeof (AlertBox);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateAlertStates();
        }

        #region Type

        public AlertType Type
        {
            get { return (AlertType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(AlertType), typeof(AlertBox), new PropertyMetadata(AlertType.Alert, UpdateAlertStates));

        private static void UpdateAlertStates(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AlertBox)d).UpdateAlertStates();
        }

        #endregion

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            Visibility = newContent == null ? Visibility.Collapsed : Visibility.Visible;
        }

        private void UpdateAlertStates()
        {
            var state = Type == AlertType.Alert ? AlertState : SuccessState;
            VisualStateManager.GoToState(this, state, false);
        }
    }
}
