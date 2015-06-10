using System.Threading;
using System.Windows.Markup;

namespace Warehouse.Wpf.Module.Main.ChangePrice
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

