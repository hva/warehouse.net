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

        #region IsWindowOpen

        public bool IsWindowOpen
        {
            get { return (bool)GetValue(IsWindowOpenProperty); }
            set { SetValue(IsWindowOpenProperty, value); }
        }

        public static readonly DependencyProperty IsWindowOpenProperty =
            DependencyProperty.Register("IsWindowOpen", typeof(bool), typeof(ParentWindowBehavior), new PropertyMetadata(false, OnIsWindowOpenChanged));

        private static void OnIsWindowOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ParentWindowBehavior)d).UpdateIsWindowOpen();
        }

        #endregion

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            window = Window.GetWindow(AssociatedObject);
            UpdateTitle();
            UpdateIsWindowOpen();
        }

        private void UpdateTitle()
        {
            if (window != null)
            {
                window.Title = Title;
            }
        }

        private void UpdateIsWindowOpen()
        {
            // is used only for closing
            if (window != null && !IsWindowOpen)
            {
                window.Close();
            }
        }
    }
}
