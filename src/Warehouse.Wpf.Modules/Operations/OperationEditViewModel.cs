using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;

namespace Warehouse.Wpf.Modules.Operations
{
    public class OperationEditViewModel : DialogViewModel
    {
        public OperationEditViewModel()
        {
            Title = "Новая накладная";
            Items = new ObservableCollection<OperationEditItem>();
            AddItemCommand = new DelegateCommand(AddItem);
        }

        public async void Init()
        {
            IsBusy = true;
            await Task.Delay(TimeSpan.FromSeconds(3));
            IsBusy = false;
        }

        public ObservableCollection<OperationEditItem> Items { get; }
        public ICommand AddItemCommand { get; }

        private void AddItem()
        {
            Items.Add(new OperationEditItem());
        }
    }
}
