using System;


namespace ProductsManagementApp.Models
{
    public class ExpirationDate : IComparable<ExpirationDate>
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public DateTime EndDate { get; set; }
        public bool Collected { get; set; }
        public DateTime CollectedDate { get; set; }
        public string CollectedBy { get; set; }
        public int? Count { get; set; }

        public ExpirationDate(int id, int productId, DateTime endDate, bool collected, DateTime collectedDate, string collectedBy, int? count)
        {
            this.Id = id;
            this.ProductId = productId;
            this.EndDate = endDate;
            this.Collected = collected;
            this.CollectedDate = collectedDate;
            this.Count = count;
            this.CollectedBy = string.Empty;
        }

        public int CompareTo(ExpirationDate expirationDate)
        {
            return DateTime.Compare(EndDate, expirationDate.EndDate);
        }
    }
}
