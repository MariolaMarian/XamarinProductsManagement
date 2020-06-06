using ProductsManagementApp.Interfaces;
using ProductsManagementApp.Models;
using ProductsManagementApp.ViewModels.Base;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProductsManagementApp.ViewModels
{
    public class ExpirationDateViewModel : ViewModelCore
    {
        private readonly IExpirationDateService _expirationDateService;

        private ExpirationDate _expirationDate;
        public ExpirationDate ExpirationDate
        {
            get { return _expirationDate; }
            set
            {
                _expirationDate = value; RaisePropertyChanged(() => ExpirationDate);
            }
        }

        public ICommand SaveCommand => new Command<ExpirationDateViewModel>(async (item) => await SaveExpDate(item));

        public ExpirationDateViewModel(IExpirationDateService expirationDateService)
        {
            _expirationDateService = expirationDateService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            if (navigationData is Product)
                ExpirationDate = new ExpirationDate(0, (navigationData as Product).Id, System.DateTime.Now, false, Convert.ToDateTime("01-01-0001"), "", null);
            else
            {
                int id = (int)navigationData;

                if (id != 0)
                {
                    ExpirationDate = await _expirationDateService.GetExpDateById(id);
                }

            }

            IsBusy = false;
        }

        private async Task SaveExpDate(ExpirationDateViewModel expDate)
        {
            ExpirationDate expirationDate = new ExpirationDate(0, ExpirationDate.ProductId, ExpirationDate.EndDate, false, new DateTime(), "", 0);
            string result = await _expirationDateService.PostAsync(expirationDate);
            if (!string.IsNullOrEmpty(result))
            {
                await DialogService.ShowAlertAsync(result.ToString(), "Zapis produktu", "Ok");
                await NavigationService.InitializeAsync();
                await NavigationService.NavigateToAsync<ProductViewModel>(ExpirationDate.ProductId);
            }
            else
                await DialogService.ShowAlertAsync("Wystąpił problem", "Zapis produktu", "Ok");
        }
    }
}