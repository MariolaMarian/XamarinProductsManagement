using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProductsManagementApp.Interfaces;
using ProductsManagementApp.ViewModels;
using ProductsManagementApp.ViewModels.Base;
using ProductsManagementApp.Views;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace ProductsManagementApp.Services
{
    public class NavigationService : INavigationService
    {
        private readonly ISettingService _settingsService;

        public NavigationService(ISettingService settingsService)
        {
            _settingsService = settingsService;
        }

        public ViewModelCore PreviousPageViewModel
        {
            get
            {
                var mainPage = Application.Current.MainPage as CustomNavigationView;
                var viewModel = mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2].BindingContext;
                return viewModel as ViewModelCore;
            }
        }
        public Task InitializeAsync()
        {
            if (Application.Current.Properties.ContainsKey("token") && Application.Current.Properties["token"] != null && !String.IsNullOrEmpty(Application.Current.Properties["token"].ToString()))
            {
                return NavigateToAsync<MenuViewModel>();
            }
            
            return NavigateToAsync<LoginViewModel>();

        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelCore
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelCore
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        private async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreatePage(viewModelType, parameter);

            
            if (page is LoginView)
            {
                Application.Current.MainPage = new CustomNavigationView(page);
            }
            else
            { 
                if (Application.Current.MainPage is CustomNavigationView navigationPage)
                {
                    if (navigationPage.CurrentPage is MasterDetailPage menuPage && menuPage.IsPresented)
                    {
                        menuPage.Detail = new CustomNavigationView(page);
                        menuPage.IsPresented = false;
                    }
                    else
                        await navigationPage.PushAsync(page);
                }
                else
                {
                    Application.Current.MainPage = new CustomNavigationView(page);
                }
            }

            await (page.BindingContext as ViewModelCore).InitializeAsync(parameter);
        }

        private Page CreatePage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            Page page = Activator.CreateInstance(pageType) as Page;
            return page;
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("Model", string.Empty);
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }

        public Task RemoveBackStackAsync()
        {
            if (Application.Current.MainPage is CustomNavigationView mainPage)
            {
                for (int i = 0; i < mainPage.Navigation.NavigationStack.Count - 1; i++)
                {
                    var page = mainPage.Navigation.NavigationStack[i];
                    mainPage.Navigation.RemovePage(page);
                }
            }

            return Task.FromResult(true);
        }

        public Task RemoveLastFromBackStackAsync()
        {

            if (Application.Current.MainPage is CustomNavigationView mainPage)
            {
                if(mainPage.Navigation.NavigationStack.Count - 2 >= 0)
                    mainPage.Navigation.RemovePage(
                        mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2]);
            }

            return Task.FromResult(true);
        }

        public async Task PushZxingPage(ZXingScannerPage page)
        {
            if (Application.Current.MainPage is CustomNavigationView navigationPage)
            {
                await navigationPage.PushAsync(page);
            }
        }

        public async Task RemoveLastAsync()
        {
            if (Application.Current.MainPage is CustomNavigationView navigationPage)
            {
                await navigationPage.PopAsync();
            }
        }
    }
}
