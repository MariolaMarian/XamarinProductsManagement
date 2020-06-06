using ProductsManagementApp.Interfaces;
using ProductsManagementApp.ViewModels.Base;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProductsManagementApp.ViewModels
{
    public class LoginViewModel: ViewModelCore
    {
       
        private readonly IUserService _userService;

        #region Properties
        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; RaisePropertyChanged(() => Email); }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; RaisePropertyChanged(() => Password); }
        }
        #endregion

        public ICommand LoginCommand => new Command(async async => await Login());

        public LoginViewModel(IUserService userService)
        {
            _userService = userService;
        }

        public override async Task InitializeAsync(object navigationData){}

        private async Task Login()
        {
            bool isConnection = false;
            IsBusy = true;

            try
            {
                await _userService.LoginAsync(this.Email, this.Password);
                isConnection = true;

            }
            catch (Exception)
            {
                await DialogService.ShowAlertAsync("Brak połączenia z internetem. Połącz się z siecią, aby móc korzystać z wszystkich funkcji aplikacji", "Błąd", "Ok");
                isConnection = false;
            }

            IsBusy = false;

            if (isConnection)
            {
                bool loginResult = Application.Current.Properties["token"] != null;

                if (loginResult)
                {
                    await NavigationService.NavigateToAsync<MenuViewModel>();
                }
                else
                {
                    string loginInfo = "Błędne dane logowania";
                    await DialogService.ShowAlertAsync(loginInfo, "Logowanie", "OK");
                }
            }

        }


    }
}
