namespace Warehouse.Wpf.UI.Modules.Shell
{
    public partial class Shell
    {
        public Shell(ShellViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        //public Shell(IEventAggregator eventAggregator) : this()
        //{
        //    eventAggregator.GetEvent<OpenWindowEvent>().Subscribe(OpenWindow, true);
        //}

        //private void OpenWindow(OpenWindowEventArgs args)
        //{
        //    var view = PageLocator.Resolve(args.PageName);

        //    var window = view as Window;
        //    if (window != null)
        //    {
        //        window.Owner = this;
        //        var vm = window.DataContext as INavigationAware;
        //        if (vm != null)
        //        {
        //            vm.OnNavigatedTo(args.Param);
        //        }
        //        window.ShowDialog();
        //    }
        //}
    }
}
