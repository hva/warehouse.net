using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Warehouse.Wpf.Controls.Behaviors
{
    public class ListBoxSelectedItemsBehavior : BehaviorBase<ListBox>
    {
        #region SelectedItems

        public object[] SelectedItems
        {
            get { return (object[])GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(IList), typeof(ListBoxSelectedItemsBehavior), new PropertyMetadata(null));

        #endregion

        protected override void OnLoaded()
        {
            AssociatedObject.SelectionChanged += OnSelectionChanged;
        }

        protected override void OnUnloaded()
        {
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var array = AssociatedObject.SelectedItems.OfType<object>().ToArray();
            SelectedItems = array;
        }
    }
}
