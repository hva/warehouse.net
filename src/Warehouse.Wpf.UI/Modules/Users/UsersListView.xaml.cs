using System.Windows.Controls;
using Microsoft.Practices.ServiceLocation;

namespace Warehouse.Wpf.UI.Modules.Users
{
    public partial class UsersListView : UserControl
    {
        public UsersListView()
        {
            InitializeComponent();

            DataContext = ServiceLocator.Current.TryResolve<UsersListViewModel>();
        }
    }
}
