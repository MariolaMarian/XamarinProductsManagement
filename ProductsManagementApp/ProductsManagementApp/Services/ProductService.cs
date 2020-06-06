using Newtonsoft.Json;
using ProductsManagementApp.Extensions;
using ProductsManagementApp.Interfaces;
using ProductsManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProductsManagementApp.Services
{
    public class ProductService : BaseHttpService, IProductService
    {
        public override string BaseUrl { get { return base.BaseUrl + "Products"; } }
        public ProductService() { }

        public async Task<List<Product>> GetAsync()
        {
            this.HttpClient.SetAuthHeader();
            var json = await HttpClient.GetStringAsync(this.BaseUrl);
            var products = JsonConvert.DeserializeObject<List<Product>>(json);
            return products;
        }

        public async Task<Product> GetProductById(int id)
        {
            var uri = new Uri(this.BaseUrl + @"/ById/" + id.ToString());

            this.HttpClient.SetAuthHeader();
            var json = await HttpClient.GetStringAsync(uri.AbsoluteUri);
            var product = JsonConvert.DeserializeObject<Product>(json);

            return product;
        }

        public async Task<string> PostAsync(Product product)
        {
            if (product.CategoryId == 0)
                product.CategoryId = 1;

            var json = JsonConvert.SerializeObject(product);
            json = json.Replace(@"\", @"\\");

            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            this.HttpClient.SetAuthHeader();
            var result = await HttpClient.PostAsync(this.BaseUrl, content);

            if (result.StatusCode == System.Net.HttpStatusCode.Created)
                return "Dodano produkt do bazy";
            return "";
        }

        public async Task<string> PutAsync(Product product)
        {
            var json = JsonConvert.SerializeObject(product);
            json = json.Replace(@"\", @"\\");

            var uri = new Uri(this.BaseUrl + @"/" + product.Id.ToString());
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            this.HttpClient.SetAuthHeader();
            var result = await HttpClient.PutAsync(uri.AbsoluteUri, content);

            if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                return "Zaktualizowano produkt";
            return "";
        }
    }
}
