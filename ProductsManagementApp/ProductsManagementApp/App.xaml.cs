using ProductsManagementApp.Interfaces;
using ProductsManagementApp.ViewModels.Base;
using ProductsManagementApp.Views;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ProductsManagementApp
{
    public partial class App : Application
    {
        ISettingService _settingsService;

        public App()
        {
            InitializeComponent();

            _settingsService = ViewModelLocator.Resolve<ISettingService>();

            //MainPage = new MainView();
        }

        protected override async void OnStart()
        {
            base.OnStart();

            await InitNavigation();

            base.OnResume();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private Task InitNavigation()
        {
            var navigationService = ViewModelLocator.Resolve<INavigationService>();
            return navigationService.InitializeAsync();
        }
    }
}
