using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Warehouse.Wpf.Module.Files
{
    public partial class FileView : UserControl
    {
        public FileView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            FixTrigger();
            FixWindowSize();
        }

        private void FixWindowSize()
        {
            // By default, PopupWindowAction creates window with SizeToContent = SizeToContent.WidthAndHeight
            // It changes window size when zooming
            // Hot fix: set window size manually
            var window = Parent as Window;
            if (window != null)
            {
                window.SizeToContent = SizeToContent.Manual;
                window.Width = 400;
                window.Height = 600;
            }
        }

        private void FixTrigger()
        {
            // InteractionRequestTrigger works only once for windows created by another InteractionRequestTrigger
            // Hot fix: attaching trigger manually
            // TODO: check why it happens
            var context = DataContext as FileViewModel;
            if (context != null)
            {
                var triggers = Interaction.GetTriggers(this);
                triggers.Clear();
                trigger.SourceObject = context.AddProductRequest;
                triggers.Add(trigger);
            }
        }
    }
}
