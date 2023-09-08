namespace Fanex.Settlement.ThirdPartyBroadcast.Shared;

public interface IBroadcastBase
{
    public string ProductType { get; set; }

    public string Data { get; set; }
}