namespace Warehouse.Wpf.Infrastructure.Interfaces
{
    public interface INavigationAware
    {
        void OnNavigatedTo(object param);
        void OnNavigatedFrom();
    }
}
