namespace Warehouse.Wpf.Module.Shell
{
    public class ShellModule // : IModule
    {
        //[Dependency]
        //public IUnityContainer Container { get; set; }

        public void Initialize()
        {
            //Container.RegisterType<object, LoginView>(Consts.LoginView);
            //Container.RegisterType<object, LoggedInView>(Consts.LoggedInView);
            //Container.RegisterType<object, TopMenu>(Consts.TopMenu);

            //ViewModelLocationProvider.Register(typeof(LoginView).FullName, () => Container.Resolve<LoginViewModel>() );
            //ViewModelLocationProvider.Register(typeof(TopMenu).FullName, () => Container.Resolve<TopMenuViewModel>());
        }
    }
}
