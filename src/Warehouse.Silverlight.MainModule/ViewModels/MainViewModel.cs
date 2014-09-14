using Microsoft.Practices.Prism.ViewModel;
using Warehouse.Silverlight.DataService;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.MainModule.ViewModels
{
    public class MainViewModel : NotificationObject // : INavigationAware 
    {
        private Taxonomy[] items;
        private readonly IDataService service;

        public MainViewModel(IDataService service)
        {
            this.service = service;
            LoadData();
        }

        public Taxonomy[] Items
        {
            get { return items; }
            set { items = value; RaisePropertyChanged(() => Items); }
        }

        private async void LoadData()
        {
            Items = await service.GetTaxonomyAsync();
        }
    }
}
