namespace Fanex.SiteDbConnectionManager.Client.Constants;

public static class CommandStatus
{
    public const int Pending = 0;

    public const int Completed = 2;

    public const int Failed = 3;
}

public static class SystemName
{
    public const string Broadcast = "Broadcast";
    public const string AutoPayout = "AutoPayout";
    public const string AutoRefund = "AutoRefund";
    public const string Web = "Web";
}