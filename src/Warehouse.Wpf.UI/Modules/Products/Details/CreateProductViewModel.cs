using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism.Events;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Events;
using Warehouse.Wpf.Models;
using Warehouse.Wpf.UI.Modules.Products.Details.Form;

namespace Warehouse.Wpf.UI.Modules.Products.Details
{
    public class CreateProductViewModel : DialogViewModel
    {
        private readonly IProductsRepository repository;
        private readonly IEventAggregator eventAggregator;

        private bool isSheet;
        private ProductFormViewModel context;

        public CreateProductViewModel(IProductsRepository repository, IEventAggregator eventAggregator)
        {
            this.repository = repository;
            this.eventAggregator = eventAggregator;

            Title = GetTitle();
        }

        public ProductFormViewModel Context
        {
            get { return context; }
            set { SetProperty(ref context, value); }
        }

        public bool IsSheet
        {
            get { return isSheet; }
            set
            {
                if (SetProperty(ref isSheet, value))
                {
                    UpdateContext();
                    UpdateTitle();
                }
            }
        }

        public void Init()
        {
            UpdateContext();
        }

        protected async override Task SaveAsync()
        {
            if (context.IsValid())
            {
                var changed = context.GetUpdatedProduct();

                IsBusy = true;
                var task = await repository.SaveAsync(changed);
                IsBusy = false;
                if (task.Succeed)
                {
                    var args = new ProductUpdatedEventArgs(task.Result, false);
                    eventAggregator.GetEvent<ProductUpdatedEvent>().Publish(args);
                    Close();
                }
            }
        }

        private void UpdateContext()
        {
            var product = new Product();
            Context = (isSheet)
                ? new SheetFormViewModel(product, true)
                : new ProductFormViewModel(product, true);
        }

        private void UpdateTitle()
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            if (window != null)
            {
                window.Title = GetTitle();
            }
        }

        private string GetTitle()
        {
            var label = isSheet ? " (лист)" : string.Empty;
            return $"Новая позиция{label}";
        }
    }
}
