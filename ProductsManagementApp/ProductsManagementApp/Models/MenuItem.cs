using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsManagementApp.Models
{
    public class MenuItem
    {
        public string Name { get; set; }
        public string ViewModel { get; set; }
        public string Icon { get; set; }
        public MenuItem(string name, string viewModel, string icon)
        {
            this.Name = name;
            this.ViewModel = viewModel;
            this.Icon = icon;
        }
    }
}
