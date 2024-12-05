namespace TRS.CoreApi.Entities;

public class AccountBalance
{
    public required Guid Id { get; set; }
    public required Guid AccountId { get; set; }
    public virtual Account? Account { get; set; }
    public required decimal Value { get; set; }
}