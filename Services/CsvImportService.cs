using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using SalesAnalysis.Data;
using SalesAnalysis.Models;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace SalesAnalysis.Services
{
    public class CsvImportService
    {
        private readonly SalesDbContext _context;

        public CsvImportService(SalesDbContext context)
        {
            _context = context;
        }

        public void ImportSalesData(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null, // Handle missing fields gracefully
                HeaderValidated = null // Disable header validation to avoid errors when headers don't match
            };

            try
            {
                using var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, config);

                var records = csv.GetRecords<dynamic>().ToList();

                foreach (var record in records)
                {
                    try
                    {
                        var recordDict = (IDictionary<string, object>)record;

                        // Parse fields and handle missing or invalid data
                        var productId = recordDict["Product ID"]?.ToString();
                        var customerId = recordDict["Customer ID"]?.ToString();
                        var orderId = int.TryParse(recordDict["Order ID"]?.ToString(), out int orderIdParsed) ? orderIdParsed : 0;
                        var quantitySold = int.TryParse(recordDict["Quantity Sold"]?.ToString(), out int quantitySoldParsed) ? quantitySoldParsed : 0;
                        var unitPrice = decimal.TryParse(recordDict["Unit Price"]?.ToString(), out decimal unitPriceParsed) ? unitPriceParsed : 0;
                        var discount = decimal.TryParse(recordDict["Discount"]?.ToString(), out decimal discountParsed) ? discountParsed : 0;
                        var shippingCost = decimal.TryParse(recordDict["Shipping Cost"]?.ToString(), out decimal shippingCostParsed) ? shippingCostParsed : 0;
                        var dateOfSale = DateTime.TryParse(recordDict["Date of Sale"]?.ToString(), out DateTime dateOfSaleParsed) ? dateOfSaleParsed : DateTime.MinValue;

                        if (string.IsNullOrEmpty(productId) || string.IsNullOrEmpty(customerId) || orderId == 0)
                        {
                            Console.WriteLine($"Skipping invalid record: {record}");
                            continue; // Skip invalid records
                        }

                        // Check if Product exists in DB, otherwise create new
                        var product = _context.Products
                            .AsNoTracking() // Avoid tracking for read operations
                            .FirstOrDefault(p => p.ProductID == productId);

                        if (product == null)
                        {
                            product = new Product
                            {
                                ProductID = productId,
                                ProductName = recordDict["Product Name"]?.ToString(),
                                Category = recordDict["Category"]?.ToString(),
                                UnitPrice = unitPrice
                            };
                            _context.Products.Add(product);
                            _context.SaveChanges(); // Save after adding the product
                        }

                        // Check if Customer exists in DB, otherwise create new
                        var customer = _context.Customers
                            .AsNoTracking() // Avoid tracking for read operations
                            .FirstOrDefault(c => c.CustomerID == customerId);

                        if (customer == null)
                        {
                            customer = new Customer
                            {
                                CustomerID = customerId,
                                CustomerName = recordDict["Customer Name"]?.ToString(),
                                CustomerEmail = recordDict["Customer Email"]?.ToString(),
                                CustomerAddress = recordDict["Customer Address"]?.ToString()
                            };
                            _context.Customers.Add(customer);
                            _context.SaveChanges(); // Save after adding the customer
                        }

                        // Create new Order (do not assign OrderID, it is an identity column)
                        var order = new Order
                        {
                            ProductID = product.ProductID,
                            CustomerID = customer.CustomerID,
                            Region = recordDict["Region"]?.ToString(),
                            DateOfSale = dateOfSale,
                            QuantitySold = quantitySold,
                            Discount = discount,
                            ShippingCost = shippingCost,
                            PaymentMethod = recordDict["Payment Method"]?.ToString()
                        };
                        _context.Orders.Add(order);
                        _context.SaveChanges(); // Save after adding the order
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing record: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing sales data: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    Console.WriteLine(ex.InnerException.StackTrace);
                }

                // Rethrow the error
                throw new Exception($"Error importing sales data: {ex.Message}", ex);
            }
        }
    }
}
