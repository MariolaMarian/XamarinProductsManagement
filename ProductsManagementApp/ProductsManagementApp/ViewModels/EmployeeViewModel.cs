using ProductsManagementApp.Interfaces;
using ProductsManagementApp.Models;
using ProductsManagementApp.ViewModels.Base;
using System.Threading.Tasks;

namespace ProductsManagementApp.ViewModels
{
    public class EmployeeViewModel:ViewModelCore
    {
        private readonly ICategoryService _categoryService;

        private Employee _employee;
        public Employee Employee
        {
            get { return _employee; }
            set { _employee = value; RaisePropertyChanged(() => Employee); }
        }

        public EmployeeViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            var categories = await _categoryService.GetAsync();

            if (navigationData != null)
                Employee = (Employee)navigationData;

            else
                 Employee = new Employee("","", "", "", "", categories);

            IsBusy = false;
        }
    }
}
