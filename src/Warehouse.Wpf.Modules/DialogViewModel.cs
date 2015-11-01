using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace Warehouse.Wpf.Modules
{
    public abstract class DialogViewModel : BindableBase, INotification, IInteractionRequestAware
    {
        private bool isBusy;
        protected DialogViewModel()
        {
            SaveCommand = DelegateCommand.FromAsyncHandler(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }

        #region INotification, IInteractionRequestAware
        public string Title { get; set; }
        public object Content { get; set; }
        public INotification Notification { get; set; }
        public Action FinishInteraction { get; set; }

        #endregion

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        protected virtual Task Save()
        {
            return Task.FromResult<object>(null);
        }

        protected void Cancel()
        {
            if (FinishInteraction != null)
            {
                FinishInteraction();
            }
        }
    }
}
