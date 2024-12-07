namespace SalesAnalysis.Models
{
    public class Product
    {
        public string ProductID { get; set; }  // Changed to string
        public string ProductName { get; set; }
        public string Category { get; set; }
        public decimal UnitPrice { get; set; }
    }

}
