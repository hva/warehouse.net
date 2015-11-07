using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Warehouse.Wpf.UI.Modules.Transactions.Details;

namespace Warehouse.Wpf.UI.Modules.Transactions
{
    public class TransactionsViewModel : BindableBase
    {
        private bool isBusy;
        private readonly InteractionRequest<TransactionDetailsViewModel> createRequest;
        private readonly Func<TransactionDetailsViewModel> editFactory;

        public TransactionsViewModel(Func<TransactionDetailsViewModel> editFactory)
        {
            this.editFactory = editFactory;
            CreateCommand = new DelegateCommand(Create);
            createRequest = new InteractionRequest<TransactionDetailsViewModel>();
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
