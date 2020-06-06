using System.Collections.Generic;

namespace ProductsManagementApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BareCode { get; set; }
        public int MaxDays { get; set; }
        public List<ExpirationDate> ExpirationDates { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public byte[] Image { get; set; }
        public bool IsVisible { get; set; }
        public Product(int Id, string Name, string BareCode, int CategoryId, int MaxDays = 7, byte[] Image=null)
        {
            this.Id = Id;
            this.Name = Name;
            this.BareCode = BareCode;
            this.CategoryId = CategoryId;
            this.MaxDays = MaxDays;
            Category = new Category();
            this.Image = Image;
            this.IsVisible = false;
            this.ExpirationDates = new List<ExpirationDate>();
        }
        public override string ToString()
        {
            return $"{Id} {Name} {BareCode} {CategoryId}";
        }
    }
}
