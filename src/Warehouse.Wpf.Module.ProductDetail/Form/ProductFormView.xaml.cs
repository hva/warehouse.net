using System.Threading;
using System.Windows.Markup;

namespace Warehouse.Wpf.Module.ProductDetail.Form
{
    public partial class ProductFormView
    {
        public ProductFormView()
        {
            InitializeComponent();
            Language = XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.Name);
        }
    }
}
