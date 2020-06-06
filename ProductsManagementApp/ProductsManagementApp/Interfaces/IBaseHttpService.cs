using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ProductsManagementApp.Interfaces
{
    interface IBaseHttpService
    {
        HttpClient HttpClient { get; set; }
        string BaseUrl { get; }
    }
}
