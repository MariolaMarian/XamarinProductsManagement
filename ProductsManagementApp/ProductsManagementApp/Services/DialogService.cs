using Acr.UserDialogs;
using ProductsManagementApp.Interfaces;
using System.Threading.Tasks;

namespace ProductsManagementApp.Services
{
    public class DialogService:IDialogService
    {
        public Task ShowAlertAsync(string message, string title, string buttonLabel)
        {
            return UserDialogs.Instance.AlertAsync(message, title, buttonLabel);
        }
    }
}
