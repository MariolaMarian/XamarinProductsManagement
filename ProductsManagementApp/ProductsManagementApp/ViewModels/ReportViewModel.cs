using ProductsManagementApp.Interfaces;
using ProductsManagementApp.Models;
using ProductsManagementApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ProductsManagementApp.ViewModels
{
    public class ReportViewModel:ViewModelCore
    {
        private readonly ICategoryService _categoryService;
        private readonly IExpirationDateService _expirationDateService;
        private readonly IProductService _productService;

        private bool _customize;
        public bool Customize
        {
            get { return _customize; }
            set { _customize = value; RaisePropertyChanged(() => Customize); }
        }

        private bool _isBeggining;
        public bool IsBeggining
        {
            get { return _isBeggining; }
            set { _isBeggining = value; RaisePropertyChanged(() => IsBeggining); }
        }

        private bool _isEnding;
        public bool IsEnding
        {
            get { return _isEnding; }
            set { _isEnding = value; RaisePropertyChanged(() => IsEnding); }
        }

        private bool _onlyCollected;
        public bool OnlyCollected
        {
            get { return _onlyCollected; }
            set { _onlyCollected = value; RaisePropertyChanged(() => OnlyCollected); }
        }


        private bool _selectedCategories;
        public bool SelectedCategories
        {
            get { return _selectedCategories; }
            set { _selectedCategories = value; RaisePropertyChanged(() => SelectedCategories); }
        }

        private ObservableCollection<Category> _allCategories;
        public ObservableCollection<Category> AllCategories
        {
            get { return _allCategories; }
            set { _allCategories = value; RaisePropertyChanged(() => AllCategories); }
        }

        private ObservableCollection<ExpirationDate> _reportData;
        public ObservableCollection<ExpirationDate> ReportData
        {
            get { return _reportData; }
            set { _reportData = value; RaisePropertyChanged(() => ReportData); }
        }

        public ReportViewModel(ICategoryService categoryService, IExpirationDateService expirationDateService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _expirationDateService = expirationDateService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            Customize = false;
            IsBeggining = false;
            IsEnding = false;
            OnlyCollected = false;
            SelectedCategories = false;

            IsBusy = true;

            var categories = await _categoryService.GetAsync();
            AllCategories = new ObservableCollection<Category>(categories);

            var products = await _productService.GetAsync();

            ReportData = new ObservableCollection<ExpirationDate>(await _expirationDateService.GetAsync(0));

            foreach(ExpirationDate expD in _reportData)
            {
                var product = products.Find(p => p.Id == expD.ProductId);
                product.Category = categories.Find(c => c.Id == product.CategoryId);
                expD.Product = product;
            }

            IsBusy = false;
        }
    }
}
