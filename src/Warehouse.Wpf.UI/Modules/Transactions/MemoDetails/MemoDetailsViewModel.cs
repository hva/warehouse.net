using System.Threading.Tasks;
using System.Windows.Controls;
using Warehouse.SharedModels;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Models;
using Warehouse.Wpf.UI.Modules.Products.Picker;

namespace Warehouse.Wpf.UI.Modules.Transactions.MemoDetails
{
    public class MemoDetailsViewModel : DialogViewModel
    {
        private bool hasContext;
        private MemoFormViewModel context;
        private readonly IProductsRepository productsRepository;

        public MemoDetailsViewModel(IProductsRepository productsRepository, ProductsPickerViewModel productsPickerViewModel)
        {
            this.productsRepository = productsRepository;
            ProductsPickerViewModel = productsPickerViewModel;

            Title = "Выбор позиции";
        }

        public ProductsPickerViewModel ProductsPickerViewModel { get; }

        public bool IsPickerVisible { get; private set; }

        public bool HasContext
        {
            get { return hasContext; }
            set { SetProperty(ref hasContext, value); }
        }

        public MemoFormViewModel Context
        {
            get { return context; }
            set { SetProperty(ref context, value); }
        }

        public string SaveButtonContent { get; private set; }

        public async void Init(MemoModel memo)
        {
            if (memo == null)
            {
                SaveButtonContent = "Добавить";
                IsPickerVisible = true;
                await ProductsPickerViewModel.InitAsync(SelectionMode.Single, OnProductSelected);
            }
            else
            {
                SaveButtonContent = "Обновить";
                Context = new MemoFormViewModel(memo);
                HasContext = true;
            }
        }

        protected override Task SaveAsync()
        {
            if (Context.IsValid())
            {
                Confirmed = true;
                Close();
            }
            return Task.FromResult<object>(null);
        }

        private async void OnProductSelected(ProductName[] products)
        {
            IsBusy = true;
            var task = await productsRepository.GetAsync(products[0].Id);
            IsBusy = false;
            if (task.Succeed)
            {
                var p = task.Result;
                Context = new MemoFormViewModel(p);
                HasContext = true;
            }
        }
    }
}
