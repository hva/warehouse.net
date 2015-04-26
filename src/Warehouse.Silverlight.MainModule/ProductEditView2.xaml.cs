using System.Threading;
using System.Windows.Markup;

namespace Warehouse.Silverlight.MainModule
{
    public partial class ProductEditView2
    {
        public ProductEditView2()
        {
            InitializeComponent();
            Language = XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.Name);
        }
    }
}
