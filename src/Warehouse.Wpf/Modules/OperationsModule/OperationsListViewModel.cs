using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;

namespace Warehouse.Wpf.Modules.OperationsModule
{
    public class OperationsListViewModel : BindableBase
    {
        private bool isBusy;
        private readonly InteractionRequest<OperationEditViewModel> createRequest;

        public OperationsListViewModel()
        {
            CreateCommand = new DelegateCommand(Create);
            createRequest = new InteractionRequest<OperationEditViewModel>();
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public ICommand CreateCommand { get; private set; }

        public IInteractionRequest CreateRequest => createRequest;

        private void Create()
        {
            createRequest.Raise(new OperationEditViewModel());
        }
    }
}
