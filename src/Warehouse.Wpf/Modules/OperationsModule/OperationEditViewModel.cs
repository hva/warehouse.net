using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace Warehouse.Wpf.Modules.OperationsModule
{
    public class OperationEditViewModel : INotification
    {
        public OperationEditViewModel()
        {
            Title = "Новая накладная";
        }

        public string Title { get; set; }
        public object Content { get; set; }
    }
}
