using Microsoft.AspNetCore.Mvc;
using SalesAnalysis.Data;
using SalesAnalysis.Services;
using System.IO;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class SalesController : ControllerBase
{
    private readonly SalesDbContext _context;
    private readonly CsvImportService _csvImportService;

    public SalesController(SalesDbContext context, CsvImportService csvImportService)
    {
        _context = context;
        _csvImportService = csvImportService;
    }

    // Endpoint to upload and import sales data from a CSV file
    [HttpPost("import")]
    public IActionResult ImportSalesData([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File not uploaded.");

        try
        {
            // Save file temporarily
            var filePath = Path.Combine(Path.GetTempPath(), file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Call the import service
            _csvImportService.ImportSalesData(filePath);

            // Optionally, delete the temporary file
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            return Ok("Data imported successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // Endpoint to calculate total revenue within a date range
    [HttpGet("revenue")]
    public IActionResult GetRevenue([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
            return BadRequest("Start date must be earlier than end date.");

        try
        {
            var revenue = _context.Orders
                .Where(o => o.DateOfSale >= startDate && o.DateOfSale <= endDate)
                .Sum(o => o.QuantitySold * o.Product.UnitPrice);

            return Ok(new { TotalRevenue = revenue });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
