namespace Autotech.Desktop.Core.Models;

public class Expense : BaseModel
{
    public string Category { get; set; } = string.Empty;
    public string? Description { get; set; }
    public double Amount { get; set; }
    public DateTime ExpenseDate { get; set; }
    public Guid? AgentId { get; set; }
    public string? ReferenceNumber { get; set; }
}
