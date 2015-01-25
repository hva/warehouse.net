using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Warehouse.Silverlight.Controls.Behaviors
{
    public class DataGridRowBackgroundBehavior : Behavior<DataGrid>
    {
        #region Converter

        public IValueConverter Converter
        {
            get { return (IValueConverter)GetValue(ConverterProperty); }
            set { SetValue(ConverterProperty, value); }
        }

        public static readonly DependencyProperty ConverterProperty =
            DependencyProperty.Register("Converter", typeof(IValueConverter), typeof(DataGridRowBackgroundBehavior), new PropertyMetadata(null));

        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.LoadingRow += LoadingRow;
            AssociatedObject.UnloadingRow += UnloadingRow;

            AssociatedObject.Unloaded += Unloaded;
        }

        private void LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (Converter == null) return;

            var brush = Converter.Convert(e.Row.DataContext, typeof(Brush), null, CultureInfo.InvariantCulture) as Brush;
            if (brush != null)
            {
                e.Row.Background = brush;
            }
        }

        private void UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Background = null;
        }

        private void Unloaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.LoadingRow -= LoadingRow;
            AssociatedObject.UnloadingRow -= UnloadingRow;
        }
    }
}
