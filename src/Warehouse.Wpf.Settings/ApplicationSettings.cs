﻿namespace Warehouse.Wpf.Settings
{
    public class ApplicationSettings : IApplicationSettings
    {
        public string Endpoint { get { return "http://localhost.fiddler:63270/"; } }
    }
}
