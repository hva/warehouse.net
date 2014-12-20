using System;

namespace Warehouse.Silverlight.Log
{
    public class BrowserLogger : ILogger
    {
        public void Log(Exception e)
        {
            try
            {
                string errorMsg = e.Message + e.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
