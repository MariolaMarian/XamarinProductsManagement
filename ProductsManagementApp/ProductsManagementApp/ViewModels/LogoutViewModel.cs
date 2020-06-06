using ProductsManagementApp.Interfaces;
using ProductsManagementApp.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProductsManagementApp.ViewModels
{
    public class LogoutViewModel : ViewModelCore
    {

        public ICommand NoCommand => new Command(async () => await Logout(false));
        public ICommand YesCommand => new Command(async () => await Logout(true));

        public LogoutViewModel()
        {
        }

        private async Task Logout(bool logout)
        {
            await NavigationService.RemoveBackStackAsync();

            if (logout)
            {
                Application.Current.Properties["token"] = null;
                await NavigationService.NavigateToAsync<LoginViewModel>();
            }
            else
            {
                await NavigationService.NavigateToAsync<MenuViewModel>();
            }
        }

    }
}
