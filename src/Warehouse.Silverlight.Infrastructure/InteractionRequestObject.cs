using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.ViewModel;

namespace Warehouse.Silverlight.Infrastructure
{
    public abstract class InteractionRequestObject : Confirmation, INotifyPropertyChanged
    {
        private bool isWindowOpen = true;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> lambda)
        {
            var name = PropertySupport.ExtractPropertyName(lambda);
            OnPropertyChanged(name);
        }

        public bool IsWindowOpen
        {
            get { return isWindowOpen; }
            set
            {
                if (isWindowOpen != value)
                {
                    isWindowOpen = value;
                    RaisePropertyChanged(() => IsWindowOpen);
                }
            }
        }

    }
}
