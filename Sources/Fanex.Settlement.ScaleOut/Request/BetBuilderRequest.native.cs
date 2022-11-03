namespace Fanex.Settlement.ScaleOut.Request;

public class BetBuilderRequest
{
    public int MatchId { get; set; }

    public string Action { get; set; }

    public bool IsAuto { get; set; }

    public string SelectedMarket { get; set; }

    public int VendorID { get; set; }
}