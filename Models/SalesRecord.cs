// SalesRecord.cs
namespace SalesAnalysis.Models
{
    public class SalesRecord
    {
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public string UnitPrice { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public string OrderID { get; set; }
        public string Region { get; set; }
        public string DateOfSale { get; set; }
        public string QuantitySold { get; set; }
        public string Discount { get; set; }
        public string ShippingCost { get; set; }
        public string PaymentMethod { get; set; }
    }
}
