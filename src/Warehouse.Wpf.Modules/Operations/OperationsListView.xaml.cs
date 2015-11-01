using Microsoft.Practices.ServiceLocation;

namespace Warehouse.Wpf.Modules.Operations
{
    public partial class OperationsListView
    {
        public OperationsListView()
        {
            InitializeComponent();

            // TODO: replace woth ViewModelLocator.AutoWireViewModel="True"
            DataContext = ServiceLocator.Current.TryResolve<OperationsListViewModel>();
        }
    }
}
