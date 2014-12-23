using System;
using System.Collections;
using System.ComponentModel;
using Microsoft.Practices.Prism.ViewModel;

namespace Warehouse.Silverlight.Infrastructure
{
    public abstract class InteractionRequestValidationObject : InteractionRequestObject, INotifyDataErrorInfo
    {
        protected ErrorsContainer<string> errorsContainer;

        protected InteractionRequestValidationObject()
        {
            errorsContainer = new ErrorsContainer<string>(RaiseErrorsChanged);
        }

        public bool HasErrors
        {
            get { return errorsContainer.HasErrors; }
        }

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
