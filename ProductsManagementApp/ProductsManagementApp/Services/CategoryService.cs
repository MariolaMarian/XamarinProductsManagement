using ProductsManagementApp.Interfaces;
using Newtonsoft.Json;
using ProductsManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ProductsManagementApp.Extensions;

namespace ProductsManagementApp.Services
{
    public class CategoryService : BaseHttpService, ICategoryService
    {
        public override string BaseUrl { get { return base.BaseUrl + "Categories"; } }
        public CategoryService()
        {
        }

        public async Task<List<Category>> GetAsync()
        {
            this.HttpClient.SetAuthHeader();
            var json = await HttpClient.GetStringAsync(this.BaseUrl);

            var categories = JsonConvert.DeserializeObject<List<Category>>(json);

            return categories;
        }

        public async Task<Category> GetCategoryByName(string name)
        {
            this.HttpClient.SetAuthHeader();
            var tmp = await GetAsync();

            foreach(Category c in tmp)
            {
                if (c.Name == name)
                {
                    return c;
                }
            }

            return new Category();
        }

        public async Task<string> PostAsync(Category category)
        {
            var content = JsonConvert.SerializeObject(category);
            this.HttpClient.SetAuthHeader();
            var result = await HttpClient.PostAsync(this.BaseUrl, new StringContent(content.ToString(), Encoding.UTF8, "application/json"));
            if (result.StatusCode == System.Net.HttpStatusCode.Created)
                return "Pomyślnie stworzono kategorię";
            return "";
        }

        public async Task<string> DeleteAsync(int id)
        {
            var uri = new Uri(new Uri(this.BaseUrl), id.ToString());
            this.HttpClient.SetAuthHeader();
            var result = await HttpClient.DeleteAsync(uri);
            return result.ToString();
        }

    }
}
