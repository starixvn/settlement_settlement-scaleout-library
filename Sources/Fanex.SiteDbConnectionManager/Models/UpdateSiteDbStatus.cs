namespace Fanex.SiteDbConnectionManager.Models;

public class UpdateSiteDbStatus
{
    public int SiteDbGroupId { get; set; }
    public string SiteName { get; set; }
    public bool Status { get; set; }
}