using System;
using System.Collections;
using System.ComponentModel;
using Prism.Mvvm;

namespace Warehouse.Wpf.UI.Modules
{
    public abstract class ValidationDialogViewModel : DialogViewModel, INotifyDataErrorInfo
    {
        protected readonly ErrorsContainer<string> errorsContainer;

        protected ValidationDialogViewModel()
        {
            errorsContainer = new ErrorsContainer<string>(RaiseErrorsChanged);
        }

        public bool HasErrors => errorsContainer.HasErrors;

        public IEnumerable GetErrors(string propertyName)
        {
            return errorsContainer.GetErrors(propertyName);
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected void RaiseErrorsChanged(string propertyName)
        {
            var handler = ErrorsChanged;
            if (handler != null)
            {
                handler(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }
    }
}
