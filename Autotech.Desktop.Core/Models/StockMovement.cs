namespace Autotech.Desktop.Core.Models;

public class StockMovement : BaseModel
{
    public Guid ItemId { get; set; }
    public Items? Item { get; set; }
    public Guid? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public string MovementType { get; set; } = string.Empty;
    public double Quantity { get; set; }
    public double UnitCost { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Notes { get; set; }
    public DateTime MovementDate { get; set; }
}
