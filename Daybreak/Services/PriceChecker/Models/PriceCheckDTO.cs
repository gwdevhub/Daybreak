using System;

namespace Daybreak.Services.PriceChecker.Models;
internal sealed class PriceCheckDTO
{
    public string? Id { get; set; }
    public DateTime? InsertionTime { get; set; }
    public double Price { get; set; }
}
