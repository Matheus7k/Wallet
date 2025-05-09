using System.Text.Json.Serialization;

namespace Wallet.Domain.Entities.v1;

public sealed class User
{
    public Guid Id { get; set; }

    [JsonIgnore]
    public Guid AddressId { get; set; }

    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Address Address { get; set; } = null!;
}