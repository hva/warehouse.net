using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace Warehouse.Wpf.UI.Modules.Shell.Login
{
    public class LoginViewModel : BindableBase
    {
        private string message;
        private string login;
        private bool isBusy;

        private Func<string, string, Task<bool>> loginCallback;

        public LoginViewModel()
        {
            LoginCommand = new DelegateCommand<PasswordBox>(DoLogin);
        }

        public LoginViewModel Init(Func<string, string, Task<bool>> callback)
        {
            loginCallback = callback;

            //string l;
            //if (IsolatedStorageSettings.ApplicationSettings.TryGetValue(Consts.SettingsLoginKey, out l))
            //{
            //    Login = l;
            //}
            //Password = string.Empty;

            return this;
        }

        public ICommand LoginCommand { get; private set; }

        public string Login
        {
            get { return login; }
            set { SetProperty(ref login, value); }
        }

        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private async void DoLogin(PasswordBox password)
        {
            Message = null;

            var task = loginCallback.Invoke(Login, password.Password);

            IsBusy = true;
            var succeed = await task;
            IsBusy = false;

            if (succeed)
            {
                //IsolatedStorageSettings.ApplicationSettings[Consts.SettingsLoginKey] = Login;
            }
            else
            {
                password.Password = string.Empty;
                Message = "Пароль, который вы ввели, неверный.\nПожалуйста, попробуйте еще раз.";
            }
        }
    }
}
