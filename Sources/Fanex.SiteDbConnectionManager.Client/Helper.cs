using System.Text.Json;

namespace Fanex.SiteDbConnectionManager.Client;

public static class Helper
{
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
}