using System.Text.Json.Serialization;

namespace Wallet.Domain.Entities.v1;

public sealed class Address
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Street { get; set; } = null!;
    public int Number { get; set; }
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
}