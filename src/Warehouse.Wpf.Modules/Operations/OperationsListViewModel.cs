using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace Warehouse.Wpf.Modules.Operations
{
    public class OperationsListViewModel : BindableBase
    {
        private bool isBusy;
        private readonly InteractionRequest<OperationEditViewModel> createRequest;
        private readonly Func<OperationEditViewModel> editFactory;

        public OperationsListViewModel(Func<OperationEditViewModel> editFactory)
        {
            this.editFactory = editFactory;
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
            var context = editFactory();
            context.Init();
            createRequest.Raise(context);
        }
    }
}
