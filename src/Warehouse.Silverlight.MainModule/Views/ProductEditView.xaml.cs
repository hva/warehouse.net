using System.ComponentModel;
using System.Threading;
using System.Windows.Markup;

namespace Warehouse.Silverlight.MainModule.Views
{
    public partial class ProductEditView
    {
        public ProductEditView()
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

