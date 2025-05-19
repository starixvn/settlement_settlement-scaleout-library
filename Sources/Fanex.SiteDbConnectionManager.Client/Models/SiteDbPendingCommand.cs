namespace Fanex.SiteDbConnectionManager.Client.Models;

public class SiteDbPendingCommand
{
    public int PId { get; set; }

    public int MatchId { get; set; }

    public string ProductType { get; set; }

    public string SpName { get; set; }

    public string Parameter { get; set; }

    public string Action { get; set; }

    public string IP { get; set; }

    public int AdminId { get; set; }

    public string SystemName { get; set; }

    public int SiteDBGroupId { get; set; }

    public int StatusId { get; set; }
}