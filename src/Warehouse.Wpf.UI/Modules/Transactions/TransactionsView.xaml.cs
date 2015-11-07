using System.Windows.Controls;
using Microsoft.Practices.ServiceLocation;

namespace Warehouse.Wpf.UI.Modules.Transactions
{
    public partial class TransactionsView : UserControl
    {
        public TransactionsView()
        {
            InitializeComponent();

            // TODO: replace woth ViewModelLocator.AutoWireViewModel="True"
            DataContext = ServiceLocator.Current.TryResolve<TransactionsViewModel>();
        }
    }
}
