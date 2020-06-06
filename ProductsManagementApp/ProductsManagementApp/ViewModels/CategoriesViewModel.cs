using ProductsManagementApp.Interfaces;
using ProductsManagementApp.Models;
using ProductsManagementApp.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProductsManagementApp.ViewModels
{
    public class CategoriesViewModel: ViewModelCore
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        ObservableCollection<GroupViewModel<Product>> _groups;
        ObservableCollection<GroupViewModel<Product>> _expandedGroups;
        public ObservableCollection<GroupViewModel<Product>> ExpandedGroups
        {
            get { return _expandedGroups; }
            set { _expandedGroups = value; RaisePropertyChanged(() => ExpandedGroups); }
        }
        public CategoriesViewModel(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _groups = new ObservableCollection<GroupViewModel<Product>>();
            ExpandedGroups = new ObservableCollection<GroupViewModel<Product>>();
        }
        internal async void HideOrShow(string categoryName)
        {
            var modifiedGroup = new ObservableCollection<GroupViewModel<Product>>();
            var category = await _categoryService.GetCategoryByName(categoryName);
            
            foreach(var gr in _groups)
            {
                var cat = new GroupViewModel<Product>(gr.Name);
                foreach(Product pr in gr)
                {
                    if(category!=null && pr.CategoryId==category.Id)
                        pr.IsVisible = !pr.IsVisible;
                    if(pr.IsVisible)
                        cat.Add(pr);
                }
                modifiedGroup.Add(cat);
            }
            ExpandedGroups = modifiedGroup;
        }
        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            var categories = await _categoryService.GetAsync();
            var products = await _productService.GetAsync();
            foreach(Product p in products)
            {
                foreach(Category c in categories)
                {
                    if(p.CategoryId==c.Id)
                    {
                        c.Products.Add(p);
                    }
                }
            }
            _groups = new ObservableCollection<GroupViewModel<Product>>();

            foreach (var category in categories)
            {
                var group = new GroupViewModel<Product>(category.Name);
                _groups.Add(group);
                foreach (var p in category.Products)
                    group.Add((p));
            }
            HideOrShow("none");
            IsBusy = false;

            await base.InitializeAsync(navigationData);
        }

        public ICommand ShowCategoryItemsCommand => new Command(ShowCategoryItems);
        public ICommand ProductDetailsCommand => new Command(async (item) => await GoToProductDetails(item));

        private void ShowCategoryItems(object ob)
        {
           HideOrShow(ob as string);
        }
        private async Task GoToProductDetails(object ob)
        {
            int productId;
            if (ob == null)
                productId = 0;
            else
                productId = ((Product)ob).Id;
            await NavigationService.NavigateToAsync<ProductViewModel>(productId);
        }
    }
}
