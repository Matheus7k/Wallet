using System.Text;
using System.Text.Json;
using Wallet.CrossCutting.Configuration.ResourceCatalog.Models;

namespace Wallet.CrossCutting.Configuration.ResourceCatalog;

public class ResourceCatalog
{
    private readonly Dictionary<string, ResourceMessage> _messages = null!;

    public ResourceCatalog()
    {
        var jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources.json");

        if (!File.Exists(jsonFilePath)) 
            return;
        
        var jsonContent = File.ReadAllText(jsonFilePath, Encoding.UTF8);
        _messages = JsonSerializer.Deserialize<Dictionary<string, ResourceMessage>>(jsonContent)!;
    }
    
    public string GetMessage(string key)
    {
        var message = _messages.TryGetValue(key, out var messages)
            ? messages.Message
            : key;

        return message!;
    }
}