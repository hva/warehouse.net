using System.ComponentModel;
using System.Threading;
using System.Windows.Markup;

namespace Warehouse.Silverlight.MainModule
{
    public partial class ProductEditView2
    {
        public ProductEditView2()
        {
            Language = XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.Name);
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            FirstTab.IsSelected = true;
        }
    }
}

