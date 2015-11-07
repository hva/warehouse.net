using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace Warehouse.Wpf.UI.Modules
{
    public abstract class DialogViewModel : BindableBase, IConfirmation, IInteractionRequestAware
    {
        private bool isBusy;
        protected DialogViewModel()
        {
            SaveCommand = DelegateCommand.FromAsyncHandler(SaveAsync);
            CancelCommand = new DelegateCommand(Close);
        }

        #region INotification, IInteractionRequestAware
        public string Title { get; set; }
        public object Content { get; set; }
        public INotification Notification { get; set; }
        public Action FinishInteraction { get; set; }
        public bool Confirmed { get; set; }
        #endregion

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        protected virtual Task SaveAsync()
        {
            return Task.FromResult<object>(null);
        }

        protected void Close()
        {
            if (FinishInteraction != null)
            {
                FinishInteraction();
            }
        }
    }
}
