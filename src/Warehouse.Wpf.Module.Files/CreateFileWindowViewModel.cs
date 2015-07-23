using System;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Win32;
using Warehouse.Wpf.Infrastructure.Interfaces;

namespace Warehouse.Wpf.Module.Files
{
    public class CreateFileWindowViewModel : BindableBase, INavigationAware
    {
        private BitmapImage imageSource;
        private string shortName;
        private string fullName;
        private string title;

        public BitmapImage ImageSource
        {
            get { return imageSource; }
            set { SetProperty(ref imageSource, value); }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public void OnNavigatedTo(object param)
        {
            var dialog = param as OpenFileDialog;
            if (dialog != null)
            {
                shortName = dialog.SafeFileName;
                fullName = dialog.FileName;
                ImageSource = new BitmapImage(new Uri(fullName));
                Title = shortName + "*";
            }
        }

        public void OnNavigatedFrom()
        {
            
        }
    }
}
