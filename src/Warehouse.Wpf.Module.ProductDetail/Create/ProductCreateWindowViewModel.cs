using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Events;
using Warehouse.Wpf.Infrastructure.Interfaces;
using Warehouse.Wpf.Models;
using Warehouse.Wpf.Module.ProductDetail.Form;

namespace Warehouse.Wpf.Module.ProductDetail.Create
{
    public class ProductCreateWindowViewModel : BindableBase, INavigationAware
    {
        private readonly IProductsRepository repository;
        private readonly IEventAggregator eventAggregator;

        private bool isSheet;
        private bool isBusy;
        private ProductFormViewModel context;
        private bool isWindowOpen = true;

        public ProductCreateWindowViewModel(IProductsRepository repository, IEventAggregator eventAggregator)
        {
            this.repository = repository;
            this.eventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(() => IsWindowOpen = false);
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public ProductFormViewModel Context
        {
            get { return context; }
            set { SetProperty(ref context, value); }
        }

        public bool IsWindowOpen
        {
            get { return isWindowOpen; }
            set { SetProperty(ref isWindowOpen, value); }
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
            set { SetProperty(ref isBusy, value); }
        }

        public bool IsSheet
        {
            get { return isSheet; }
            set
            {
                if (SetProperty(ref isSheet, value))
                {
                    UpdateContext();
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
                    //Confirmed = true;
                    IsWindowOpen = false;
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

        public void OnNavigatedTo()
        {
            UpdateContext();
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
