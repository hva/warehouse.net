using System.Threading;
using System.Windows.Markup;

namespace Warehouse.Silverlight.MainModule
{
    public partial class ProductCreateWindow
    {
        public ProductCreateWindow()
        {
            Language = XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.Name);
            InitializeComponent();
        }
    }
}

