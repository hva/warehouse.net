using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Infrastructure.Interfaces;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.Module.Files
{
    public class EditFileViewModel : CreateFileViewModel
    {
        private readonly IApplicationSettings settings;

        public EditFileViewModel(IFilesRepository filesRepository, Func<ProductPickerViewModel> pickerFactory, IApplicationSettings settings)
            : base(filesRepository, pickerFactory)
        {
            this.settings = settings;
        }

        public void Init(FileDescription file, IDictionary<string, string> names)
        {
            if (file.Metadata != null)
            {
                var products = from x in  file.Metadata.ProductIds
                               select new ProductName { Id = x, Name = names[x] };
                Products.AddRange(products);
            }

            Title = file.Name;
            var uriString = string.Concat(settings.Endpoint, "api/files/", file.Id);
            var uri = new Uri(uriString, UriKind.Absolute);
            ImageSource = new BitmapImage(uri);
        }
    }
}
