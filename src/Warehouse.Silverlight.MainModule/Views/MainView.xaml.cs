using System.Windows.Controls;
using Warehouse.Silverlight.MainModule.ViewModels;

namespace Warehouse.Silverlight.MainModule.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }

        public MainView(MainViewModel context) : this()
        {
            DataContext = context;
        }
    }
}
