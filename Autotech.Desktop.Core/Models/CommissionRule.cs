namespace Autotech.Desktop.Core.Models;

public class CommissionRule : BaseModel
{
    public Guid? AgentId { get; set; }
    public double RatePercent { get; set; }
    public bool AppliesToNetSales { get; set; } = true;
    public bool IsActive { get; set; } = true;
    public DateTime EffectiveDate { get; set; }
}
