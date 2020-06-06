using ProductsManagementApp.Interfaces;
using ProductsManagementApp.Models;
using ProductsManagementApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProductsManagementApp.ViewModels
{
    public class CategoryViewModel: ViewModelCore, IEquatable<CategoryViewModel>
    {
        private readonly ICategoryService _categoryService;

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged(() => Id); }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(() => Name); }
        }
        private ObservableCollection<ProductViewModel> _products;
        public ObservableCollection<ProductViewModel> Products
        {
            get { return _products; }
            set { _products = value; RaisePropertyChanged(() => Products); }
        }
        private bool _productsVisible;
        public bool ProductsVisible {
            get { return _productsVisible; }
            set { _productsVisible = value; RaisePropertyChanged(() => ProductsVisible); }
        }

        public CategoryViewModel(ICategoryService categoryService) : this(categoryService, 0, "", new ObservableCollection<ProductViewModel>()){}

        public CategoryViewModel(ICategoryService categoryService, int id, string name, ObservableCollection<ProductViewModel> products, bool visible = false)
        {
            this._categoryService = categoryService;
            this.Id = id;
            this.Name = name;
            this.Products = products;
            this.ProductsVisible = visible;
        }

        public override string ToString()
        {
            return $"{Id} {Name}";
        }

        public bool Equals(CategoryViewModel other)
        {
            if (other == null) return false;
            return this.Id.Equals(other.Id);
        }

        public ICommand SaveCommand => new Command<CategoryViewModel>(async (item) => await Save(item));

        private async Task Save(CategoryViewModel category)
        {
            Category savedCategory = new Category(category.Id,category.Name);
            string result = await _categoryService.PostAsync(savedCategory);
            if (String.IsNullOrEmpty(result))
                result = "Nie powiodło się stworzenie kategorii";
            else
            {
                this.Name = "";
                this.Id = 0;
                await NavigationService.RemoveLastFromBackStackAsync();
                await NavigationService.NavigateToAsync<CategoriesViewModel>();
            }
            await DialogService.ShowAlertAsync(result, "Zapis kategorii", "Ok");
        }
    }
}