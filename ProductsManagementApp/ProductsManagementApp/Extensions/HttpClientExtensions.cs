using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;

namespace ProductsManagementApp.Extensions
{
    public static class HttpClientExtensions
    {
        public static HttpClient SetAuthHeader(this HttpClient httpClient)
        {
            if (Application.Current.Properties.ContainsKey("token") && Application.Current.Properties["token"] != null && !String.IsNullOrEmpty(Application.Current.Properties["token"].ToString()))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"].ToString());
            }
            return httpClient;
        }
    }
}
