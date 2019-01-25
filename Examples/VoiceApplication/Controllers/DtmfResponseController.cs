namespace Voice.Controllers
{
    public class DtmfResponse
    {
        public string callerNumber { get; set; }
        public string dtmfDigits { get; set; }
        public string callStartTime {get; set; }
        public string callerCountryCode {get; set;}
        public string destinationNumber { get; set; }
        public string direction { get; set; }
        public string isActive { get; set; }
        public string sessionId { get; set; }
    }
}