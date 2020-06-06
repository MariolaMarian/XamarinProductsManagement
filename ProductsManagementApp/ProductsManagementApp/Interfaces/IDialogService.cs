using System.Threading.Tasks;

namespace ProductsManagementApp.Interfaces
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string buttonLabel);
    }
}
