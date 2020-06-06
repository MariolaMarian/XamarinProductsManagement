using System.Collections.Generic;

namespace ProductsManagementApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Product> Products { get; set; }

        public Category()
        {
            this.Id = 0;
            this.Name = "brak";
            this.Products = new List<Product>();
        }

        public Category(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
