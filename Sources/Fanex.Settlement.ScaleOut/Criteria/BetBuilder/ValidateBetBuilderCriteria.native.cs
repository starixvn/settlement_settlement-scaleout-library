namespace Fanex.Settlement.ScaleOut.Criteria.BetBuilder;

public class ValidateBetBuilderCriteria : ExecuteSingleCriteria
{
    public int MatchId { get; set; }

    public string Action { get; set; }

    public bool IsAuto { get; set; }

    public string SelectedMarket { get; set; }

    public int VendorId { get; set; }

    public string InvalidMarketId { get; set; }

    public override string GetSettingKey() => "NAP_Admin_Settlement_Betbuilder_Validation_Event";

    public override bool IsValid() => MatchId > 0;
}