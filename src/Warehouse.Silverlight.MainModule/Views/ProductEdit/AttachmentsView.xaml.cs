using System.Threading;
using System.Windows.Markup;

namespace Warehouse.Silverlight.MainModule.Views.ProductEdit
{
    public partial class AttachmentsView
    {
        public AttachmentsView()
        {
            Language = XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.Name);
            InitializeComponent();
        }
    }
}
