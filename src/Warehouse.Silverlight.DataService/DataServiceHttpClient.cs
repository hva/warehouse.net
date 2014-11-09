using System.Net.Http;

namespace Warehouse.Silverlight.DataService
{
    public class DataServiceHttpClient : HttpClient
    {
        public DataServiceHttpClient()
        {
            BaseAddress = System.Windows.Browser.HtmlPage.Document.DocumentUri;
        }
    }
}
