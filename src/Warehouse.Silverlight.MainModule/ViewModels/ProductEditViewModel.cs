using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Practices.Prism.Commands;
using Warehouse.Silverlight.MainModule.Infrastructure;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.MainModule.ViewModels
{
    public class ProductEditViewModel : InteractionRequestValidationObject
    {
        private string name;

        public ProductEditViewModel(Product product)
        {
            name = product.Name;

            Title = string.Format("{0} {1}", product.Name, product.Size);
            SaveCommand = new DelegateCommand<ChildWindow>(Save);
        }

        public ICommand SaveCommand { get; private set; }

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    ValidateName();
                }
            }
        }

        private async void Save(ChildWindow window)
        {
            ValidateName();

            if (HasErrors) return;

            // prevent closing while awaiting
            window.Closing += WindowClosing;

            await TaskEx.Delay(TimeSpan.FromSeconds(5));

            window.Closing -= WindowClosing;

            //Confirmed = true;
            //window.Close();
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void ValidateName()
        {
            errorsContainer.ClearErrors(() => Name);
            errorsContainer.SetErrors(() => Name, Validate.Required(Name));
        }
    }
}
