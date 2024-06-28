namespace AppCore.Models;

public class CreateTicketModel
{
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public DateOnly ExpirationDate { get; set; }
    public DateOnly ValidityStartDate {  get; set; }
}
