using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Infrastructure.Events;
using Warehouse.Wpf.Models;
using Warehouse.Wpf.Module.ProductDetail.Form;

namespace Warehouse.Wpf.Module.ProductDetail.Create
{
    public class ProductCreateWindowViewModel : InteractionRequestValidationObject, INavigationAware
    {
        private readonly IProductsRepository repository;
        private readonly IEventAggregator eventAggregator;

        private bool isSheet;
        private bool isBusy;
        private ProductFormViewModel context;

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
                ? new SheetFormViewModel(product, true)
                : new ProductFormViewModel(product, true);
        }

        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            UpdateContext();
            IsWindowOpen = true;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        #endregion
    }
}
