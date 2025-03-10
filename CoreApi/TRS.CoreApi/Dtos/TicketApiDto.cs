﻿using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Dtos;

public class TicketApiDto(Ticket ticket)
{
    public Guid TicketId { get; set; } = ticket.Id;
    public string Name { get; set; } = ticket.Name;
    public decimal Price { get; set; } = ticket.Price;
    public DateOnly ExpirationDate { get; set; } = ticket.ExpirationDate;
    public DateOnly ValidityStartDate { get; set; } = ticket.ValidityStartDate;
}
