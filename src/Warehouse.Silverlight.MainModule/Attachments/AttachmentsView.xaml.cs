using System.Threading;
using System.Windows.Markup;

namespace Warehouse.Silverlight.MainModule.Attachments
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
