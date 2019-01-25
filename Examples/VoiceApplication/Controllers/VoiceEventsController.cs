namespace Voice.Controllers
{
    public class VoiceEvents
    {
        public string sessionId { get; set; }
        public string isActive { get; set; }
        public string direction {get; set; }
        public string callerNumber {get; set; }
        public string callStartTime {get; set; }
        public string callerCountryCode {get; set;}
        public string amount {get; set;}
        public string callSessionState { get; set; }
        public string status { get; set; }
    }
}