using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Warehouse.Wpf.Auth;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Events;
using Warehouse.Wpf.Infrastructure.Interfaces;
using Warehouse.Wpf.Models;
using Warehouse.Wpf.Module.ProductDetail.Form;
//using Warehouse.Wpf.Module.Main.Attachments;

namespace Warehouse.Wpf.Module.ProductDetail.Edit
{
    public class ProductEditWindowViewModel : BindableBase, INavigationAware
    {
        private bool isBusy;
        private bool isWindowOpen = true;
        private ProductFormViewModel context;
        private string title;

        private readonly bool canSave;
        private readonly bool canEditPrice;

        private readonly IProductsRepository repository;
        private readonly IEventAggregator eventAggregator;
        //private readonly AttachmentsViewModel attachmentsViewModel;

        public ProductEditWindowViewModel(IProductsRepository repository, IEventAggregator eventAggregator,
            IAuthStore authStore/*, AttachmentsViewModel attachmentsViewModel*/)
        {
            this.repository = repository;
            this.eventAggregator = eventAggregator;
            //this.attachmentsViewModel = attachmentsViewModel;

            var token = authStore.LoadToken();
            if (token != null)
            {
                canSave = token.IsEditor();
                canEditPrice = token.IsAdmin();
            }

            SaveCommand = new DelegateCommand(Save, () => canSave);
            CancelCommand = new DelegateCommand(() => IsWindowOpen = false);
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        //public AttachmentsViewModel AttachmentsContext { get { return attachmentsViewModel; } }

        public ProductFormViewModel Context
        {
            get { return context; }
            set { SetProperty(ref context, value); }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public bool IsWindowOpen
        {
            get { return isWindowOpen; }
            set { SetProperty(ref isWindowOpen, value); }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private async void Save()
        {
            if (Context.IsValid())
            {
                var changed = Context.GetUpdatedProduct();

                IsBusy = true;
                var task = await repository.SaveAsync(changed);
                IsBusy = false;
                if (task.Succeed)
                {
                    var args = new ProductUpdatedEventArgs(task.Result, false);
                    eventAggregator.GetEvent<ProductUpdatedEvent>().Publish(args);
                    IsWindowOpen = false;
                }
            }
        }

        //private async void InitAttachments(string id)
        //{
        //    await attachmentsViewModel.Init(id);
        //}

        public void OnNavigatedTo(object param)
        {
            var p = (Product) param;

            Title = string.Format("{0} {1}", p.Name, p.Size);

            Context = (p.IsSheet)
                ? new SheetFormViewModel(p, canEditPrice)
                : new ProductFormViewModel(p, canEditPrice);

            //InitAttachments(p.Id);
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
