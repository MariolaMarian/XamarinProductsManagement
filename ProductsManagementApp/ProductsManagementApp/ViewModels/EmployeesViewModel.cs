using ProductsManagementApp.Interfaces;
using ProductsManagementApp.Models;
using ProductsManagementApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProductsManagementApp.ViewModels
{
    public class EmployeesViewModel : ViewModelCore
    {
        private readonly ICategoryService _categoryService;

        public ICommand ShowEmployeeCommand => new Command(async(item) => await ShowEmployee(item));

        private ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set { _employees = value; RaisePropertyChanged(() => Employees); }
        }

        public EmployeesViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;

            Employees = new ObservableCollection<Employee>();
        }

        private async Task ShowEmployee(object employee)
        {
            if(employee is Employee)
                await NavigationService.NavigateToAsync<EmployeeViewModel>(employee);
            else
                await NavigationService.NavigateToAsync<EmployeeViewModel>(null);
        }

        public override async Task InitializeAsync(object navigationData)
        {
            List<Category> categories = await _categoryService.GetAsync();

            IsBusy = true;

            Employees.Add(new Employee("11223","Mariusz", "Kowalski", "mkowalski@gmail.com", "haslo", categories));
            Employees.Add(new Employee("112232","Aldona", "Nowak", "anowak@gmail.com", "xdfsfd", categories));
            Employees.Add(new Employee("355","Wojciech", "Tajny", "tajniak@gmail.com", "dgdgdg", categories));
            Employees.Add(new Employee("1123244","Zygmunt", "Polak", "zigi@gmail.com", "gdgsdgdgdgd", categories));

            IsBusy = false;
        }
    }
}
