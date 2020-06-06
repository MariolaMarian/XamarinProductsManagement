using ProductsManagementApp.Converters;
using ProductsManagementApp.Interfaces;
using ProductsManagementApp.Models;
using ProductsManagementApp.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ProductsManagementApp.ViewModels
{
    public class ProductsViewModel: ViewModelCore
    {
        private readonly IProductService _productService;
        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { _products = value; RaisePropertyChanged(() => Products); }
        }
        public string Header { get; set; }

        public ProductsViewModel(IProductService productService)
        {
            _productService = productService;
            Products = new ObservableCollection<Product>();
        }

        public  async Task InitializeProductsAsync()
        {
            Products = ListToObservableConverter<Product>.ToObservable(await _productService.GetAsync());
        }
    }
}
