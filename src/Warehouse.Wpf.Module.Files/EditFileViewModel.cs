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
    public class EditFileViewModel : FileViewModel
    {
        private readonly IFilesRepository filesRepository;
        private readonly IApplicationSettings settings;
        private string fileId;

        public EditFileViewModel(IFilesRepository filesRepository, IApplicationSettings settings, Func<ProductPickerViewModel> pickerFactory)
            : base(pickerFactory)
        {
            this.filesRepository = filesRepository;
            this.settings = settings;
        }

        public void Init(FileDescription file, IDictionary<string, string> names)
        {
            fileId = file.Id;

            if (file.Metadata != null)
            {
                string str;
                var products = from x in file.Metadata.ProductIds
                               let name = names.TryGetValue(x, out str) ? str : null
                               where name != null
                               select new ProductName { Id = x, Name = name };
                Products.AddRange(products);
            }

            Title = file.Name;
            var uriString = string.Concat(settings.Endpoint, "api/files/", file.Id);
            var uri = new Uri(uriString, UriKind.Absolute);
            ImageSource = new BitmapImage(uri);
        }

        protected async override void Save()
        {
            IsBusy = true;

            var productIds = Products.Select(x => x.Id).ToArray();
            var task = await filesRepository.AttachProducts(fileId, productIds);
            if (task.Succeed)
            {
                Confirmed = true;
                Close();
            }

            IsBusy = false;

        }
    }
}
