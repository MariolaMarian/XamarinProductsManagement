using System.Threading.Tasks;

namespace ProductsManagementApp.Interfaces
{
    public interface IUserService
    {
        Task LoginAsync(string login, string password);
    }
}
