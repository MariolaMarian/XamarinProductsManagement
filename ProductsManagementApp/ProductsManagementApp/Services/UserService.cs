using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProductsManagementApp.Extensions;
using ProductsManagementApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProductsManagementApp.Services
{
    class UserService : BaseHttpService, IUserService
    {

        public UserService() { }

        public async Task LoginAsync(string login, string password)
        {
            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username",login),
                new KeyValuePair<string, string>("password",password),
                new KeyValuePair<string, string>("grant_type","password")
            };

            var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl.Replace(@"api/", "Token"))
            {
                Content = new FormUrlEncodedContent(keyValues)
            };

            var client = new HttpClient();
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            JObject jwtDynamic = JsonConvert.DeserializeObject<dynamic>(content);
            var accessToken = jwtDynamic.Value<string>("access_token");

            Application.Current.Properties["isManager"] = false;

            if (accessToken!=null)
            {
                Application.Current.Properties["token"] = accessToken;
  

                var uri = new Uri( BaseUrl + @"Account/UserInfo");
                this.HttpClient.SetAuthHeader();
                var json = await HttpClient.GetStringAsync(uri.AbsoluteUri);
                var result = JsonConvert.DeserializeObject<dynamic>(json);
                Application.Current.Properties["isManager"] = result.Value<bool>("IsManager");
            }
            else
            {
                Application.Current.Properties["token"] = null;
            }
        }
    }
}
