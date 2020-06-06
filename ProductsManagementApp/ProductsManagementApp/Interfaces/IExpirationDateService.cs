

using ProductsManagementApp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ProductsManagementApp.Interfaces
{
    public interface IExpirationDateService
    {
        Task<List<ExpirationDate>> GetAsync(int id);
        Task<ExpirationDate> GetExpDateById(int id);
        Task<string> PutAsync(ExpirationDate expirationDate);
        Task<string> PostAsync(ExpirationDate expirationDate);
    }
}
