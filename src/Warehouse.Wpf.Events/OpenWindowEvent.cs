using Prism.Events;

namespace Warehouse.Wpf.Events
{
    public class OpenWindowEvent: PubSubEvent<OpenWindowEventArgs>
    {
    }

    public class OpenWindowEventArgs
    {
        public OpenWindowEventArgs(string pageName, object param)
        {
            PageName = pageName;
            Param = param;
        }
        public string PageName { get; private set; }
        public object Param { get; private set; }
    }
}
