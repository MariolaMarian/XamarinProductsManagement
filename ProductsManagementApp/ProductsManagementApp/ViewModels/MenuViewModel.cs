using ProductsManagementApp.Interfaces;
using ProductsManagementApp.Models;
using ProductsManagementApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using MenuItem = ProductsManagementApp.Models.MenuItem;

namespace ProductsManagementApp.ViewModels
{
    public class MenuViewModel: ViewModelCore
    {
        INotificationService _notificationService;
        IProductService _productService;
        IExpirationDateService _expirationDateService;

        public List<MenuItem> MenuItems { get; set; }
        public MenuViewModel(IProductService productService, IExpirationDateService expirationDateService)
        {
            MenuItems = new List<MenuItem>
            {
                new MenuItem("Produkty", nameof(CategoriesViewModel), "productsIcon.png"),
                new MenuItem("Dodaj produkt", nameof(ProductViewModel), "fruitIcon.png"),
                new MenuItem("Dodaj kategorię", nameof(CategoryViewModel), "categoriesIcon.png"),
                new MenuItem("Daty przydatności", nameof(ExpirationDatesViewModel), "dateIcon.png")
            };
            if (Application.Current.Properties["isManager"] != null && (bool)Application.Current.Properties["isManager"]==true)
            {
                MenuItems.Add(new MenuItem("Raporty", nameof(ReportViewModel), "reportIcon.png"));
                MenuItems.Add(new MenuItem("Pracownicy", nameof(EmployeesViewModel), "usersIcon.png"));
            }
            MenuItems.Add(new MenuItem("Wyloguj", nameof(LogoutViewModel), "logoutIcon.png"));

            _productService = productService;
            _expirationDateService = expirationDateService;
            _notificationService = DependencyService.Get<INotificationService>();

        }

        public override async Task InitializeAsync(object navigationData)
        {
            await base.InitializeAsync(navigationData);

            List<ExpirationDate> expirationDates = new List<ExpirationDate>();
            List<Product> products = new List<Product>();

            try {
                expirationDates = await _expirationDateService.GetAsync(0);
                products = await _productService.GetAsync();
            }
            catch (Exception)
            {
                await NavigationService.RemoveBackStackAsync();
                await NavigationService.NavigateToAsync<LoginViewModel>();
            }

            var datesToNotification = new List<ExpirationDate>();

            foreach(ExpirationDate expD in expirationDates)
            {
                if (!expD.Collected)
                {
                    expD.Product = products.Find(p => p.Id == expD.ProductId);
                    datesToNotification.Add(expD);
                }
            }

            foreach(ExpirationDate expD in datesToNotification)
            {
                _notificationService.ScheduleNotification(expD.Product.Name + " " + expD.Product.BareCode, "Kończąca się data przydatności " + expD.EndDate);
            }

        }

        public ICommand NavigateDetailCommand => new Command<MenuItem>(async (item) => await NavigateToDetail(item));

        private async Task NavigateToDetail(MenuItem item)
        {
            
            switch (item.ViewModel)
            {
                case "CategoriesViewModel":
                    await NavigationService.NavigateToAsync<CategoriesViewModel>();
                    break;
                case "ProductViewModel":
                    await NavigationService.NavigateToAsync<ProductViewModel>(0);
                    break;
                case "CategoryViewModel":
                    await NavigationService.NavigateToAsync<CategoryViewModel>(0);
                    break;
                case "ExpirationDatesViewModel":
                    await NavigationService.NavigateToAsync<ExpirationDatesViewModel>();
                    break;
                case "ReportViewModel":
                    await NavigationService.NavigateToAsync<ReportViewModel>();
                    break;
                case "EmployeesViewModel":
                    await NavigationService.NavigateToAsync<EmployeesViewModel>();
                    break;
                case "LogoutViewModel":
                    await NavigationService.NavigateToAsync<LogoutViewModel>();
                    break;

            }
        }
    }
}
