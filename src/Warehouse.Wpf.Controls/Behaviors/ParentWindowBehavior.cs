using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Warehouse.Wpf.Controls.Behaviors
{
    public class ParentWindowBehavior : Behavior<UserControl>
    {
        private Window window;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
        }

        #region Title

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ParentWindowBehavior), new PropertyMetadata(null, OnTitleChanged));

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ParentWindowBehavior)d).UpdateTitle();
        }

        #endregion

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            window = Window.GetWindow(AssociatedObject);
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            if (window != null)
            {
                window.Title = Title;
            }
        }
    }
}
