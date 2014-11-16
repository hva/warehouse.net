using System.Net.Http;

namespace Warehouse.Silverlight.DataService.Http
{
    public class BaseHttpClient : HttpClient
    {
        public BaseHttpClient()
        {
            BaseAddress = System.Windows.Browser.HtmlPage.Document.DocumentUri;
        }
    }
}
