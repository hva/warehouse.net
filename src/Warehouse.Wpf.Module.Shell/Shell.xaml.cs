using System.Windows;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Warehouse.Wpf.Events;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Infrastructure.Interfaces;

namespace Warehouse.Wpf.Module.Shell
{
    public partial class Shell : IView
    {
        public Shell()
        {
            InitializeComponent();
        }

        public Shell(IEventAggregator eventAggregator) : this()
        {
            eventAggregator.GetEvent<OpenWindowEvent>().Subscribe(OpenWindow, true);
        }

        private void OpenWindow(OpenWindowEventArgs args)
        {
            var view = PageLocator.Resolve(args.PageName);

            var window = view as Window;
            if (window != null)
            {
                window.Owner = this;
                var vm = window.DataContext as INavigationAware;
                vm?.OnNavigatedTo(args.Param);
                window.ShowDialog();
            }
        }
    }
}
