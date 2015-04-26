using System.ComponentModel;

namespace Warehouse.Silverlight.MainModule
{
    public partial class ProductEditWindow
    {
        public ProductEditWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            TabControl.SelectedIndex = 0;
        }
    }
}

