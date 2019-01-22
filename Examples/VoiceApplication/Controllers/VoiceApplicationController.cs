using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.IO;

namespace Voice.Controllers
{
    [Route("service")]
    public class VoiceApplicationController : ApiController
    {
        private readonly string xmlHeader = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
        
        [Route("voice")] // https://www.mydomain.com/service/voice // http:1.2.3.4:8008/service/voice
        [HttpPost]
        public HttpResponseMessage appHttpResponseMessage([FromBody] VoiceResponse voiceResponse)
        {
            HttpResponseMessage responseMessage;
            // The Default action here is to <Redirect> smartly...
            // Redirect to outbound handler or the call is outbound
            // Redirect to inbound hander if the call is inbound
            // The value is always derived from the call direction parameter
            string appUrl = hostNameResolver();

            // You can save the session ID as well
            //string sessionId = voiceResponse.sessionId;
            string defaultVoiceAction = "";
            if (voiceResponse.isActive == "1")
            {
            switch (voiceResponse.direction)
            {
                case "Inbound":
                    defaultVoiceAction = $"{xmlHeader}<Response><Redirect>http://{appUrl}/service/inbound</Redirect></Response>";
                    break;
                case "Outbound":
                    defaultVoiceAction = $"{xmlHeader}<Response><Redirect>http://{appUrl}/service/outbound</Redirect><Response>";
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
            }
            responseMessage = Request.CreateResponse(HttpStatusCode.Created, defaultVoiceAction);
            responseMessage.Content = new StringContent(defaultVoiceAction, Encoding.UTF8, "application/xml");
            return responseMessage;
        }

        [Route("outbound")]
        [HttpPost]

        public HttpResponseMessage outboundHttpResponseMessage([FromBody] VoiceResponse voiceResponse)
        {
            HttpResponseMessage responseMessage;
            string defaultVoiceAction = "";
            if (voiceResponse.isActive == "1")
            {
                defaultVoiceAction = sampleOutboundResponse();
                responseMessage = Request.CreateResponse(HttpStatusCode.Created, defaultVoiceAction);
                responseMessage.Content = new StringContent(defaultVoiceAction, Encoding.UTF8, "application/xml");
                return responseMessage;
            } else {
                // do something else, the call is complete
                return null;

            }
        }

        [Route("inbound")]
        [HttpPost]
       public HttpResponseMessage inboundHttpResponseMessage([FromBody] VoiceResponse voiceResponse)
        {
            HttpResponseMessage responseMessage;
            string defaultVoiceAction = "";
            if (voiceResponse.isActive == "1")
            {
                defaultVoiceAction = sampleInboundAction();
                responseMessage = Request.CreateResponse(HttpStatusCode.Created, defaultVoiceAction);
                responseMessage.Content = new StringContent(defaultVoiceAction, Encoding.UTF8, "application/xml");
                return responseMessage;
            } else {
                // do something else, the call is complete
                return null;
            }
        }

        [Route("dtmf")]
        [HttpPost]

        public HttpResponseMessage dtmfHttpResponseMessage([FromBody] VoiceResponse voiceResponse)
        {
            HttpResponseMessage responseMessage;
            string defaultVoiceAction = "";
            if (voiceResponse.dtmfDigits != null)
            {
                defaultVoiceAction = finalDtmf(voiceResponse.callerNumber);
                responseMessage = Request.CreateResponse(HttpStatusCode.Created, defaultVoiceAction);
                responseMessage.Content = new StringContent(defaultVoiceAction, Encoding.UTF8, "application/xml");
                return responseMessage;
            } else {
                // do something else, the call is complete

            }
            return null;
        }

        public string sampleOutboundResponse()
        {
            // You can fetch number to dial or connect to a call from the DB etc 
            // The can be your agents
            // In this case we'll be dialing our SIP agent on a soft-phone
            string appHostname = hostNameResolver();
            // string mySipNumber = "test.kennedy@ke.sip.africastalking.com";
            string outboundDialAction = $"{xmlHeader}<Response><Play ur=\"http://{appHostname}/Static/IndianaCut.mp3\" />";
            // string outboundDialAction = $"{xmlHeader}<Response><Dial record=\"true\" phoneNumbers=\"{mySipNumber}\" /> </Response>";
            return outboundDialAction;
        }

        
        public string sampleInboundAction()
        {
            string appHostname = hostNameResolver();
            string sayActionPrompt = "<Say voice=\"man\"> Please enter your PIN to continue. Press the asterisk sign to finish</Say>";
            string sayActionTimeout = "<Say voice=\"man\"> Am sorry we did not get that. Good bye</Say>";
            string getDigitsAction = $"<GetDigits numDigits=\"4\" finishOnKey=\"*\" callbackUrl=\"http://{appHostname}/service/voice/dtmf\" timeout=\"30\" >{sayActionPrompt}</GetDigits>";
            string getDigitsActionRes = $"{xmlHeader}<Response>{getDigitsAction}{sayActionTimeout}</Response>";
            return getDigitsActionRes;
        }

        public string finalDtmf(string phoneNumber)
        {
            string phoneNum = phoneNumber;
            string dtmfGreeter = $"{xmlHeader}<Response><Say voice=\"man\">Hello {phoneNum}, thank you</Say></Response>";
            return dtmfGreeter;
        }

        public string hostNameResolver()
        {
            string uri = "http://ifconfig.me";
            var getUrlReq = (HttpWebRequest)WebRequest.Create(uri);
            getUrlReq.UserAgent = "curl";
            getUrlReq.Method = "GET";

            using (WebResponse webResponse = getUrlReq.GetResponse())
            {
                using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    string readerVal = streamReader.ReadToEnd();
                    return (readerVal.Replace("\n",""));
                }
            }
        }
    }
}