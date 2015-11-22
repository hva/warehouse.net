using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Warehouse.Wpf.Auth;
using Warehouse.Wpf.Auth.Interfaces;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Models;
using Warehouse.Wpf.UI.Modules.Transactions.MemoDetails;

namespace Warehouse.Wpf.UI.Modules.Transactions.Details
{
    public class TransactionDetailsViewModel : ValidationDialogViewModel
    {
        private string id;
        private string customer;
        private object[] selectedItems;
        private bool isDraft;

        private readonly InteractionRequest<MemoDetailsViewModel> memoCreateRequest;
        private readonly InteractionRequest<MemoDetailsViewModel> memoEditRequest;
        private readonly ITransactionsRepository transactionsRepository;
        private readonly Func<MemoDetailsViewModel> memoFactory;
        private readonly DelegateCommand deleteItemsCommand;

        public TransactionDetailsViewModel(IAuthStore authStore, ITransactionsRepository transactionsRepository,
            Func<MemoDetailsViewModel> memoFactory)
        {
            this.transactionsRepository = transactionsRepository;
            this.memoFactory = memoFactory;

            Items = new ObservableCollection<MemoModel>();
            OpenItemCommand = new DelegateCommand<MemoModel>(OpenItem);
            memoCreateRequest = new InteractionRequest<MemoDetailsViewModel>();
            memoEditRequest = new InteractionRequest<MemoDetailsViewModel>();
            AddItemCommand = new DelegateCommand(AddItem);
            deleteItemsCommand = new DelegateCommand(DeleteItems, CanDeleteItems);

            var token = authStore.LoadToken();
            if (token != null && token.IsAuthenticated())
            {
                UserName = token.UserName;
            }
        }

        public IInteractionRequest MemoCreateRequest => memoCreateRequest;
        public IInteractionRequest MemoEditRequest => memoEditRequest;
        public ICommand AddItemCommand { get; }
        public ICommand DeleteItemsCommand => deleteItemsCommand;
        public ICommand OpenItemCommand { get; }
        public string UserName { get; }

        public string SaveButtonContent => isDraft ? "Сохранить" : "Отправить на склад";
        public bool IsNotDraft => !isDraft;
        public bool IsForSale { get; set; }

        public bool IsDraft
        {
            get { return isDraft; }
            set
            {
                if (SetProperty(ref isDraft, value))
                {
                    OnPropertyChanged(() => SaveButtonContent);
                    OnPropertyChanged(() => IsNotDraft);
                }
            }
        }

        public void Init(TransactionModel transaction)
        {
            if (transaction == null)
            {
                Title = "Расходная накладная - Создание";
                IsForSale = true;
            }
            else
            {
                id = transaction.Id;
                Title = "Расходная накладная - Редактирование";
                Customer = transaction.Customer;
                Items.AddRange(transaction.Memos);
                SetStatus(transaction.Status);
            }
        }

        public object[] SelectedItems
        {
            get {  return selectedItems; }
            set { selectedItems = value; deleteItemsCommand.RaiseCanExecuteChanged(); }
        }

        #region Delete

        private void DeleteItems()
        {
            var items = selectedItems.OfType<MemoModel>().ToArray();
            foreach (var item in items)
            {
                Items.Remove(item);
            }
        }

        private bool CanDeleteItems()
        {
            return selectedItems != null && selectedItems.OfType<MemoModel>().Any();
        }

        #endregion

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

        protected async override Task SaveAsync()
        {
            ValidateCustomer();
            ValidateItems();

            var t = new TransactionModel
            {
                Id = id,
                DateTime = DateTime.UtcNow,
                Employee = UserName,
                Customer = customer,
                Memos = Items.ToArray(),
                Status = GetStatus()
            };

            IsBusy = true;
            var task = await transactionsRepository.SaveAsync(t);
            IsBusy = false;

            if (task.Succeed)
            {
                Confirmed = true;
            }
            Close();
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

        private string GetStatus()
        {
            if (isDraft)
            {
                return "Draft";
            }
            return IsForSale ? "ForSale" : "Reserved";
        }

        private void SetStatus(string status)
        {
            if (status == "Draft")
            {
                isDraft = true;
                IsForSale = true;
            }
            else
            {
                IsForSale = (status == "ForSale");
            }
        }
    }
}
