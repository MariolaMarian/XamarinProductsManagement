using ProductsManagementApp.ViewModels.Base;
using System.Threading.Tasks;
using ZXing.Net.Mobile.Forms;

namespace ProductsManagementApp.Interfaces
{
    public interface INavigationService
    {
        ViewModelCore PreviousPageViewModel { get; }
        Task InitializeAsync();
        Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelCore;
        Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelCore;
        Task RemoveLastFromBackStackAsync();
        Task RemoveBackStackAsync();
        Task PushZxingPage(ZXingScannerPage page);
        Task RemoveLastAsync();
    }
}
