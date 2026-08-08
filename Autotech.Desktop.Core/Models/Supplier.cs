namespace Autotech.Desktop.Core.Models;

public class Supplier : BaseModel
{
    public string SupplierName { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime DateAdded { get; set; }
}
