namespace Fanex.Settlement.ScaleOut.Models;
public class InvalidMarkets(string marketIds, string noResultMarketIds)
{
    public string MarketIds { get; set; } = marketIds;

    public string NoResultMarketIds { get; set; } = noResultMarketIds;
}
