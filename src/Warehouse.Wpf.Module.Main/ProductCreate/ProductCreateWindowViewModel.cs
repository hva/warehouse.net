using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Infrastructure.Events;
using Warehouse.Wpf.Models;
using Warehouse.Wpf.Module.Main.ProductEdit;

namespace Warehouse.Wpf.Module.Main.ProductCreate
{
    public class ProductCreateWindowViewModel : InteractionRequestValidationObject
    {
        private readonly IProductsRepository repository;
        private readonly IEventAggregator eventAggregator;

        private bool isSheet;
        private bool isBusy;
        private ProductEditViewModel2 context;

        public ProductCreateWindowViewModel(IProductsRepository repository, IEventAggregator eventAggregator)
        {
            this.repository = repository;
            this.eventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(() => IsWindowOpen = false);
        }

        public ProductCreateWindowViewModel Init()
        {
            UpdateContext();
            IsWindowOpen = true;
            return this;
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public ProductEditViewModel2 Context
        {
            get { return context; }
            set { context = value; RaisePropertyChanged(() => Context); }
        }

        public string Title2
        {
            get
            {
                var label = isSheet ? " (лист)" : string.Empty;
                return string.Format("Новая позиция{0}", label);
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        public bool IsSheet
        {
            get { return isSheet; }
            set
            {
                if (isSheet != value)
                {
                    isSheet = value;
                    UpdateContext();
                    RaisePropertyChanged(() => Title2);
                }
            }
        }

        private async void Save()
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
                    Confirmed = true;
                    IsWindowOpen = false;
                }
            }
        }

        private void UpdateContext()
        {
            var product = new Product();
            Context = (isSheet)
                ? new SheetEditViewModel2(product, true)
                : new ProductEditViewModel2(product, true);
        }
    }
}
