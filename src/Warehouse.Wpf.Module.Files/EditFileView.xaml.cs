using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Warehouse.Wpf.Module.Files
{
    public partial class EditFileView : UserControl
    {
        public EditFileView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // InteractionRequestTrigger works only once for windows created by another InteractionRequestTrigger
            // Hot fix: attaching trigger manually
            // TODO: check why it happens
            var context = DataContext as CreateFileViewModel;
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
