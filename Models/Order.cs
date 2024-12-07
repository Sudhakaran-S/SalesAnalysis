using SalesAnalysis.Models;

public class Order
{
    public int OrderID { get; set; }
    public string ProductID { get; set; }  // Assuming ProductID is now a string
    public string CustomerID { get; set; } // Assuming CustomerID is now a string
    public string Region { get; set; }
    public DateTime DateOfSale { get; set; }
    public int QuantitySold { get; set; }
    public decimal Discount { get; set; }
    public decimal ShippingCost { get; set; }
    public string PaymentMethod { get; set; }

    // Add this navigation property to reference the related Product
    public virtual Product Product { get; set; }  // Virtual for lazy loading
}
