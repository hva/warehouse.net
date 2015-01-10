using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.MainModule.ViewModels
{
    public class ChangePriceViewModel : InteractionRequestObject
    {
        private string percentage = "10";

        public ChangePriceViewModel(IEnumerable<Product> products)
        {
            SaveCommand = new DelegateCommand<ChildWindow>(Save);

            LoadItems(products);
            UpdatePrice();
        }

        public ICommand SaveCommand { get; private set; }
        public ObservableCollection<ChangePriceItem> Items { get; private set; }

        public string Percentage
        {
            get { return percentage; }
            set
            {
                if (percentage != value)
                {
                    percentage = value;
                    UpdatePrice();
                }
            }
        }

        private void LoadItems(IEnumerable<Product> products)
        {
            var items =
                from p in products
                select new ChangePriceItem
                {
                    Id = p.Id,
                    Name = p.Name,
                    Size = p.Size,
                    PriceOpt = p.PriceOpt,
                };

            Items = new ObservableCollection<ChangePriceItem>(items);
        }

        private void UpdatePrice()
        {
            double p;
            double.TryParse(percentage, out p);
            foreach (var x in Items)
            {
                x.Refresh(p);
            }
        }

        private void Save(ChildWindow window)
        {
            window.Close();
        }
    }
}
