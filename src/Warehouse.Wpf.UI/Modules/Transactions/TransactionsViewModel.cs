using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure.Interfaces;
using Warehouse.Wpf.Models;
using Warehouse.Wpf.UI.Modules.Transactions.Details;

namespace Warehouse.Wpf.UI.Modules.Transactions
{
    public class TransactionsViewModel : BindableBase, INavigationAware
    {
        private bool isBusy;
        private object[] selectedItems;
        private readonly InteractionRequest<TransactionDetailsViewModel> detailsRequest;
        private readonly ObservableCollection<TransactionModel> items;
        private readonly CollectionViewSource cvs;
        private readonly DelegateCommand deleteCommand;

        private readonly ITransactionsRepository transactionsRepository;
        private readonly Func<TransactionDetailsViewModel> editFactory;

        public TransactionsViewModel(ITransactionsRepository transactionsRepository, Func<TransactionDetailsViewModel> editFactory)
        {
            this.transactionsRepository = transactionsRepository;
            this.editFactory = editFactory;

            CreateCommand = new DelegateCommand(Create);
            OpenCommand = new DelegateCommand<TransactionModel>(Open);
            deleteCommand = DelegateCommand.FromAsyncHandler(DeleteAsync, CanDelete);
            detailsRequest = new InteractionRequest<TransactionDetailsViewModel>();
            items = new ObservableCollection<TransactionModel>();
            cvs = new CollectionViewSource { Source = items };
            cvs.SortDescriptions.Add(new SortDescription(nameof(TransactionModel.DateTime), ListSortDirection.Descending));
            cvs.SortDescriptions.Add(new SortDescription(nameof(TransactionModel.Employee), ListSortDirection.Ascending));
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public ICommand CreateCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand DeleteCommand => deleteCommand;
        public IInteractionRequest CreateRequest => detailsRequest;
        public ICollectionView Items => cvs.View;

        public object[] SelectedItems
        {
            get { return selectedItems; }
            set { selectedItems = value; deleteCommand.RaiseCanExecuteChanged(); }
        }

        #region INavigationAware

        public async void OnNavigatedTo(object param)
        {
            await RefreshAsync();
        }

        public void OnNavigatedFrom()
        {
        }

        #endregion

        private async Task RefreshAsync()
        {
            IsBusy = true;
            var task = await transactionsRepository.GetAsync();
            IsBusy = false;

            if (task.Succeed)
            {
                items.Clear();
                items.AddRange(task.Result);
            }
        }

        private void Create()
        {
            var context = editFactory();
            context.Init(null);
            detailsRequest.Raise(context, OnCreated);
        }

        private async void OnCreated(TransactionDetailsViewModel vm)
        {
            if (vm.Confirmed)
            {
                await RefreshAsync();
            }
        }

        private void Open(TransactionModel transaction)
        {
            var context = editFactory();
            context.Init(transaction);
            detailsRequest.Raise(context, OnCreated);
        }

        private async Task DeleteAsync()
        {
            var ids = selectedItems.OfType<TransactionModel>().Select(x => x.Id).ToArray();
            var task = await transactionsRepository.DeleteAsync(ids);
            if (task.Succeed)
            {
                await RefreshAsync();
            }
        }

        private bool CanDelete()
        {
            return selectedItems != null && selectedItems.OfType<TransactionModel>().Any();
        }
    }
}
