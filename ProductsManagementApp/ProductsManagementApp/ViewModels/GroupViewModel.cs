using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ProductsManagementApp.ViewModels
{
    public class GroupViewModel<T>:ObservableCollection<T>
    {
        public string Name { get; set; }
        public GroupViewModel(string name)
        {
            this.Name = name;
        }
    }
}
