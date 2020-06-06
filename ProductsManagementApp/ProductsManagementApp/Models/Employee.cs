using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsManagementApp.Models
{
    public class Employee
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Category> Categories { get; set; }

        public Employee(string id, string firstName, string lastName, string email, string password, List<Category> categories)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Categories = categories;
            
        }
    }
}
