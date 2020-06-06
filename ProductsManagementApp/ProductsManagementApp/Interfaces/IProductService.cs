
using ProductsManagementApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsManagementApp.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetAsync();
        Task<Product> GetProductById(int id);
        Task<string> PostAsync(Product product);
        Task<string> PutAsync(Product product);
    }
}
