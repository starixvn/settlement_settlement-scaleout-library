namespace Fanex.SiteDbConnectionManager.Models;

public class SiteDbPendingCommand
{
    public int PId { get; set; }

    public int MatchId { get; set; }

    public string SportType { get; set; }

    public string SpName { get; set; }

    public string Parameter { get; set; }

    public string Action { get; set; }

    public string IP { get; set; }

    public string AdminId { get; set; }

    public bool IsAPI { get; set; }

    public string SiteDBGroupId { get; set; }

    public int StatusId { get; set; }
}