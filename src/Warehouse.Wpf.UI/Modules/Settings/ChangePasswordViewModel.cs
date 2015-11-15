using System;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure;

namespace Warehouse.Wpf.UI.Modules.Settings
{
    public class ChangePasswordViewModel : BindableBase, IConfirmation, IInteractionRequestAware
    {
        private bool isBusy;
        private string errorMessage;
        private string userName;
        private readonly IUsersRepository usersRepository;

        public ChangePasswordViewModel(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
            Title = "Смена пароля - ";
            SaveCommand = new DelegateCommand<ChangePasswordArgs>(Save);
            CloseCommand = new DelegateCommand(Close);
        }

        public ChangePasswordViewModel Init(string userNameParam)
        {
            userName = userNameParam;
            Title += userNameParam;
            return this;
        }

        #region IConfirmation, IInteractionRequestAware
        public string Title { get; set; }
        public object Content { get; set; }
        public bool Confirmed { get; set; }
        public INotification Notification { get; set; }
        public Action FinishInteraction { get; set; }
        #endregion

        public ICommand SaveCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }


        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }

        private async void Save(ChangePasswordArgs args)
        {
            ErrorMessage = null;

            if (IsFormValid(args))
            {
                IsBusy = true;
                var task = await usersRepository.ChangePasswordAsync(userName, args.Old.Password, args.New.Password);
                IsBusy = false;
                if (task.Succeed)
                {
                    Confirmed = true;
                    Close();
                }
                else
                {
                    ErrorMessage = task.ErrorMessage;
                }
            }
        }

        private bool IsFormValid(ChangePasswordArgs args)
        {
            if (string.IsNullOrWhiteSpace(args.Old.Password))
            {
                ErrorMessage = "введите старый пароль";
                return false;
            }

            if (string.IsNullOrWhiteSpace(args.New.Password))
            {
                ErrorMessage = "введите новый пароль";
                return false;
            }

            var errors = Validate.Password(args.New.Password, args.New2.Password).ToArray();
            if (errors.Length > 0)
            {
                ErrorMessage = errors[0];
                return false;
            }

            return true;
        }

        private void Close()
        {
            if (FinishInteraction != null)
            {
                FinishInteraction();
            }
        }

    }
}
