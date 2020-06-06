using ProductsManagementApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsManagementApp.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAsync();
        Task<Category> GetCategoryByName(string name);
        Task<string> PostAsync(Category category);
        Task<string> DeleteAsync(int id);
    }
}

