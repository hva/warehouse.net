using System;
using System.Linq;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Warehouse.Wpf.Data.Interfaces;

namespace Warehouse.Wpf.Module.Files
{
    public class CreateFileViewModel : FileViewModel
    {
        private OpenFileDialog dialog;
        private readonly IFilesRepository filesRepository;

        public CreateFileViewModel(IFilesRepository filesRepository, Func<ProductPickerViewModel> pickerFactory)
            : base(pickerFactory)
        {
            this.filesRepository = filesRepository;
        }

        public void Init(OpenFileDialog d)
        {
            dialog = d;
            ImageSource = new BitmapImage(new Uri(dialog.FileName));
            Title = dialog.SafeFileName + "*";
        }

        protected async override void Save()
        {
            if (dialog != null)
            {
                IsBusy = true;

                var task = await filesRepository.Create(dialog.OpenFile(), dialog.SafeFileName, "image/jpeg");
                if (task.Succeed)
                {
                    var fileId = task.Result;
                    var productIds = Products.Select(x => x.Id).ToArray();
                    var task2 = await filesRepository.AttachProducts(fileId, productIds);
                    if (task2.Succeed)
                    {
                        Confirmed = true;
                        Close();
                    }
                }

                IsBusy = false;
            }
        }
    }
}
