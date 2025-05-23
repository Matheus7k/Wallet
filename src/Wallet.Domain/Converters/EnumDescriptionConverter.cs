using System.Text.Json;
using System.Text.Json.Serialization;
using Wallet.Domain.Extensions;

namespace Wallet.Domain.Converters;

public class EnumDescriptionConverter<T> : JsonConverter<T> where T : Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => (T)Enum.Parse(typeof(T), reader.GetString()!);

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.GetDescription());
}