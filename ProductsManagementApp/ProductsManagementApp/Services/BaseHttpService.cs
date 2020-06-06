using ProductsManagementApp.Interfaces;
using ProductsManagementApp.ViewModels.Base;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;

namespace ProductsManagementApp.Services
{
    public abstract class BaseHttpService : IBaseHttpService
    {
        public HttpClient HttpClient { get; set; } 
        public virtual string BaseUrl { get { return "http://productsxamarinapi.hostingasp.pl/api/"; } }

        public BaseHttpService()
        {
            HttpClient = new HttpClient();
        }

    }
}
