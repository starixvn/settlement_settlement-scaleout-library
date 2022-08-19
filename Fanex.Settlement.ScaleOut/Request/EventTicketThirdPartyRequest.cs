namespace Fanex.Settlement.ScaleOut.Request
{
    public class EventTicketThirdPartyRequest
    {
        public int MatchId { get; set; }

        public int VendorId { get; set; }

        public string Action { get; set; }

        public string Mode { get; set; }

        public bool IsAuto { get; set; }

        public string SelectedResources { get; set; }

        public string SpName { get; set; }
    }
}
