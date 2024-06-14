namespace DidacticVerse.Accounts;

public class RefreshTokens
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public bool Consumed { get; set; } = false;
    public long AccountId { get; set; }

    public bool Invalidated { get; set; } = false;
    public string? Reconsumption { get; set; }
    public DateTime? ReconsumptionTime { get; set; }
}
