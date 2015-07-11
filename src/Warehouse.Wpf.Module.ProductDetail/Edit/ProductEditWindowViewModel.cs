using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using Warehouse.Wpf.Auth;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Infrastructure.Events;
using Warehouse.Wpf.Models;
using Warehouse.Wpf.Module.ProductDetail.Form;
//using Warehouse.Wpf.Module.Main.Attachments;

namespace Warehouse.Wpf.Module.ProductDetail.Edit
{
    public class ProductEditWindowViewModel : InteractionRequestValidationObject
    {
        private readonly IProductsRepository repository;
        private readonly IEventAggregator eventAggregator;
        //private readonly AttachmentsViewModel attachmentsViewModel;

        private readonly bool canSave;
        private readonly bool canEditPrice;

        private bool isBusy;
        private ProductFormViewModel context;

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

        public ProductEditWindowViewModel Init(Product p)
        {
            Title = string.Format("{0} {1}", p.Name, p.Size);

            Context = (p.IsSheet)
                ? new SheetFormViewModel(p, canEditPrice)
                : new ProductFormViewModel(p, canEditPrice);

            //InitAttachments(p.Id);

            IsWindowOpen = true;
            return this;
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        //public AttachmentsViewModel AttachmentsContext { get { return attachmentsViewModel; } }

        public ProductFormViewModel Context
        {
            get { return context; }
            set { context = value; RaisePropertyChanged(() => Context); }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; RaisePropertyChanged(() => IsBusy); }
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
                    Confirmed = true;
                    IsWindowOpen = false;
                }
            }
        }

        //private async void InitAttachments(string id)
        //{
        //    await attachmentsViewModel.Init(id);
        //}
    }
}
