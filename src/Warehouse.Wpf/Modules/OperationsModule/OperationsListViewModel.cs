using Microsoft.Practices.Prism.Mvvm;

namespace Warehouse.Wpf.Modules.OperationsModule
{
    public class OperationsListViewModel : BindableBase
    {
        private bool isBusy;

        public OperationsListViewModel()
        {
            
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
    }
}
