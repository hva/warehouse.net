using System.Threading;
using System.Windows.Markup;

namespace Warehouse.Silverlight.MainModule.Views
{
    public partial class ChangePriceWindow
    {
        public ChangePriceWindow()
        {
            InitializeComponent();
            Language = XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.Name);
        }
    }
}

