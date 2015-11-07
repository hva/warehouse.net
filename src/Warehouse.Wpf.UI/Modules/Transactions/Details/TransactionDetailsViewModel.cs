using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Warehouse.SharedModels;
using Warehouse.Wpf.Auth;
using Warehouse.Wpf.Auth.Interfaces;
using Warehouse.Wpf.UI.Modules.Transactions.MemoDetails;

namespace Warehouse.Wpf.UI.Modules.Transactions.Details
{
    public class TransactionDetailsViewModel : DialogViewModel
    {
        private readonly InteractionRequest<MemoDetailsViewModel> memoDetailsRequest;
        private readonly IAuthStore authStore;
        private readonly Func<MemoDetailsViewModel> memoFactory;

        public TransactionDetailsViewModel(IAuthStore authStore, Func<MemoDetailsViewModel> memoFactory)
        {
            this.authStore = authStore;
            this.memoFactory = memoFactory;

            Title = "Расходная накладная";
            Items = new ObservableCollection<MemoModel>();
            OpenItemCommand = new DelegateCommand<MemoModel>(OpenItem);
            memoDetailsRequest = new InteractionRequest<MemoDetailsViewModel>();
            AddItemCommand = new DelegateCommand(AddItem);
        }

        public IInteractionRequest MemoDetailsRequest => memoDetailsRequest;
        public ICommand AddItemCommand { get; }

        public void Init()
        {
            var token = authStore.LoadToken();
            if (token != null && token.IsAuthenticated())
            {
                UserName = token.UserName;
            }
        }

        public ObservableCollection<MemoModel> Items { get; }
        public ICommand OpenItemCommand { get; }
        public string UserName { get; private set; }

        private void AddItem()
        {
            var context = memoFactory();
            context.Init(null);
            memoDetailsRequest.Raise(context, OnItemAdded);
        }

        private void OnItemAdded(MemoDetailsViewModel vm)
        {
            if (vm.Confirmed)
            {
                Items.Add(vm.Context.GetUpdatedMemo());
            }
        }

        private void OpenItem(MemoModel memo)
        {
            var context = memoFactory();
            context.Init(memo);
            memoDetailsRequest.Raise(context, OnItemUpdated);
        }

        private void OnItemUpdated(MemoDetailsViewModel vm)
        {
            if (vm.Confirmed)
            {
                var memo = vm.Context.GetUpdatedMemo();
                var index = Items.IndexOf(memo);
                Items.RemoveAt(index);
                Items.Insert(index, memo);
            }
        }
    }
}
