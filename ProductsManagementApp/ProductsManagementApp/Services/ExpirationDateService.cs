using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProductsManagementApp.Extensions;
using ProductsManagementApp.Interfaces;
using ProductsManagementApp.Models;

namespace ProductsManagementApp.Services
{
    public class ExpirationDateService : BaseHttpService, IExpirationDateService
    {
        public override string BaseUrl { get { return base.BaseUrl + "ExpirationDates"; } }
        public ExpirationDateService() { }

        public async Task<List<ExpirationDate>> GetAsync(int productId=0)
        {
            this.HttpClient.SetAuthHeader();
            var json = await HttpClient.GetStringAsync(BaseUrl);
            var expirationDates = JsonConvert.DeserializeObject<List<ExpirationDate>>(json);
            if (productId != 0)
            {
                List<ExpirationDate> tmp = new List<ExpirationDate>();

                foreach(ExpirationDate exd in expirationDates)
                {
                    if (exd.ProductId == productId)
                    {
                        tmp.Add(exd);
                    }
                }

                expirationDates = tmp;
            }
            return expirationDates;
        }

        public async Task<ExpirationDate> GetExpDateById(int id)
        {
            var uri = new Uri(this.BaseUrl + @"/" + id.ToString());
            this.HttpClient.SetAuthHeader();
            var json = await HttpClient.GetStringAsync(uri.AbsoluteUri);
            var expDate = JsonConvert.DeserializeObject<ExpirationDate>(json);

            return expDate;
        }

        public async Task<string> PutAsync(ExpirationDate expirationDate)
        {

            var json = JsonConvert.SerializeObject(expirationDate);
            json = json.Replace(@"\", @"\\");

            var uri = new Uri(this.BaseUrl + @"/" + expirationDate.Id.ToString());
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            this.HttpClient.SetAuthHeader();
            var result = await HttpClient.PutAsync(uri.AbsoluteUri, content);

            if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                return "Zebrano produkty";
            return "";
        }

        public async Task<string> PostAsync(ExpirationDate expirationDate)
        {
            var json = JsonConvert.SerializeObject(expirationDate);
            json = json.Replace(@"\", @"\\");

            var uri = new Uri(this.BaseUrl);
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            this.HttpClient.SetAuthHeader();
            var result = await HttpClient.PostAsync(uri.AbsoluteUri, content);

            if (result.StatusCode == System.Net.HttpStatusCode.Created)
                return "Dodano datę przydatności";
            return "";
        }
    }
}
