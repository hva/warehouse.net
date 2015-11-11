using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Warehouse.SharedModels;
using Warehouse.Wpf.Auth;
using Warehouse.Wpf.Auth.Interfaces;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.UI.Modules.Transactions.MemoDetails;

namespace Warehouse.Wpf.UI.Modules.Transactions.Details
{
    public class TransactionDetailsViewModel : ValidationDialogViewModel
    {
        private string customer;

        private readonly InteractionRequest<MemoDetailsViewModel> memoCreateRequest;
        private readonly InteractionRequest<MemoDetailsViewModel> memoEditRequest;
        private readonly IAuthStore authStore;
        private readonly Func<MemoDetailsViewModel> memoFactory;

        public TransactionDetailsViewModel(IAuthStore authStore, Func<MemoDetailsViewModel> memoFactory)
        {
            this.authStore = authStore;
            this.memoFactory = memoFactory;

            Title = "Расходная накладная";
            Items = new ObservableCollection<MemoModel>();
            OpenItemCommand = new DelegateCommand<MemoModel>(OpenItem);
            memoCreateRequest = new InteractionRequest<MemoDetailsViewModel>();
            memoEditRequest = new InteractionRequest<MemoDetailsViewModel>();
            AddItemCommand = new DelegateCommand(AddItem);
        }

        public IInteractionRequest MemoCreateRequest => memoCreateRequest;
        public IInteractionRequest MemoEditRequest => memoEditRequest;
        public ICommand AddItemCommand { get; }

        public void Init()
        {
            var token = authStore.LoadToken();
            if (token != null && token.IsAuthenticated())
            {
                UserName = token.UserName;
            }
        }

        public ICommand OpenItemCommand { get; }
        public string UserName { get; private set; }

        #region Items

        public ObservableCollection<MemoModel> Items { get; }

        private void ValidateItems()
        {
            errorsContainer.ClearErrors(() => Items);
            if (Items.Count == 0)
            {
                errorsContainer.SetErrors(() => Items, new[] { "список не может быть пустым" });
            }
        }

        #endregion

        #region Customer

        public string Customer
        {
            get { return customer; }
            set
            {
                if (customer != value)
                {
                    customer = value;
                    ValidateCustomer();
                }
            }
        }

        private void ValidateCustomer()
        {
            errorsContainer.ClearErrors(() => Customer);
            errorsContainer.SetErrors(() => Customer, Validate.Required(Customer));
        }

        #endregion

        protected override Task SaveAsync()
        {
            ValidateCustomer();
            ValidateItems();

            return base.SaveAsync();
        }

        private void AddItem()
        {
            var context = memoFactory();
            context.Init(null);
            memoCreateRequest.Raise(context, OnItemAdded);
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
            memoEditRequest.Raise(context, OnItemUpdated);
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
