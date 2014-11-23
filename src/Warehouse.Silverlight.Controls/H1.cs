using System.Windows;
using System.Windows.Controls;

namespace Warehouse.Silverlight.Controls
{
    public class H1 : ContentControl
    {
        public H1()
        {
            DefaultStyleKey = typeof (H1);
        }

        #region Title

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(H1), new PropertyMetadata(null));

        #endregion
    }
}
