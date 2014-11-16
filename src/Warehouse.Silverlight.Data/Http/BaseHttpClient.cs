using System.Net.Http;

namespace Warehouse.Silverlight.Data.Http
{
    public class BaseHttpClient : HttpClient
    {
        public BaseHttpClient()
        {
            BaseAddress = System.Windows.Browser.HtmlPage.Document.DocumentUri;
        }
    }
}
