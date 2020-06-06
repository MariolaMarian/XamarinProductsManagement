using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ProductsManagementApp.Converters;
using ProductsManagementApp.Interfaces;
using ProductsManagementApp.Models;
using ProductsManagementApp.ViewModels.Base;

namespace ProductsManagementApp.ViewModels
{
    public class ExpirationDatesViewModel:ViewModelCore
    {
        private readonly IExpirationDateService _expirationDatesService;
        private readonly IProductService _productService;

        private ObservableCollection<ExpirationDate> _expirationDates;
        public ObservableCollection<ExpirationDate> ExpirationDates
        {
            get { return _expirationDates; }
            set { _expirationDates = value; RaisePropertyChanged(() => ExpirationDates); }
        }

        public ExpirationDatesViewModel(IExpirationDateService expirationDatesService, IProductService productService)
        {
            _expirationDatesService = expirationDatesService;
            _productService = productService;
            ExpirationDates = new ObservableCollection<ExpirationDate>();
        }

        public async override Task InitializeAsync(object navigationData)
        {
            ExpirationDates = new ObservableCollection<ExpirationDate>();
            try
            {
                var tmpDates = await _expirationDatesService.GetAsync(0);
                var tmpProducts = await _productService.GetAsync();

                foreach(ExpirationDate exd in tmpDates)
                {
                    exd.Product = tmpProducts.Find(p => p.Id == exd.ProductId);
                }

                ExpirationDates = ListToObservableConverter<ExpirationDate>.ToObservable(tmpDates);

            }
            catch(Exception)
            {
                await DialogService.ShowAlertAsync("Nie powiodło się pobieranie danych","Błąd","Ok");
            }
        }
    }
}
