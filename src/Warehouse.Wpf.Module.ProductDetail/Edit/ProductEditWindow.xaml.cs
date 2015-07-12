using Microsoft.Practices.Prism.Mvvm;

namespace Warehouse.Wpf.Module.ProductDetail.Edit
{
    public partial class ProductEditWindow : IView
    {
        public ProductEditWindow()
        {
            InitializeComponent();
        }

        //protected override void OnClosing(CancelEventArgs e)
        //{
        //    base.OnClosing(e);
        //    TabControl.SelectedIndex = 0;
        //}
    }
}

