using System.Threading;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Warehouse.Silverlight.MainModule.Views
{
    public partial class ProductEditView : ChildWindow
    {
        public ProductEditView()
        {
            Language = XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.Name);
            InitializeComponent();
        }
    }
}

