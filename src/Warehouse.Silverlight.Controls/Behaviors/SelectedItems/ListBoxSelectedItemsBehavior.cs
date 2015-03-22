using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Warehouse.Silverlight.Controls.Behaviors
{
    public class ListBoxSelectedItemsBehavior : Behavior<ListBox>
    {
        #region SelectedItems

        public IList SelectedItems
        {
            get { return (IList)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(IList), typeof(ListBoxSelectedItemsBehavior), new PropertyMetadata(null));

        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += OnSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var array = AssociatedObject.SelectedItems.OfType<object>().ToArray();
            SelectedItems = array;
        }
    }
}
