namespace Autotech.Desktop.Core.Models;

public class ProductReturn : BaseModel
{
    public Guid ItemId { get; set; }
    public Items? Item { get; set; }
    public Guid? SalesId { get; set; }
    public string ReturnType { get; set; } = string.Empty;
    public double Quantity { get; set; }
    public string? Reason { get; set; }
    public string Status { get; set; } = "Open";
    public DateTime ReturnDate { get; set; }
}
