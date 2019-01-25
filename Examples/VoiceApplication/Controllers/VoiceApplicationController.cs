using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Voice.Controllers
{
    
    [Route("service")]
    public class VoiceApplicationController : ApiController
    {
        
        private readonly string xmlHeader = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
        
        [Route("voice")] // https://www.mydomain.com/service/voice // http:1.2.3.4:8008/service/voice
        [HttpPost]

        public HttpResponseMessage appHttpResponseMessage([FromForm] VoiceResponse voiceResponse)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            // The Default action here is to <Redirect> smartly...
            // Redirect to outbound handler or the call is outbound
            // Redirect to inbound hander if the call is inbound
            // The value is always derived from the call direction parameter
            string appUrl = hostNameResolver();
            //string appHostname = "https://cdb5482a.ngrok.io";
            // This is a very dangerous hack strive to do something cleaner
            // // You can save the session ID as well
            //string sessionId = voiceResponse.sessionId;
            string defaultVoiceAction = "";
            if (voiceResponse.isActive == "1")
            {
            switch (voiceResponse.direction)
            {
                case "Inbound":
                    //defaultVoiceAction = $"{xmlHeader}<Response><Redirect>{appHostname}/service/inbound</Redirect></Response>";
                    defaultVoiceAction = $"{xmlHeader}<Response><Redirect>http://{appUrl}:7380/service/inbound</Redirect></Response>";
                    break;
                case "Outbound":
                    //defaultVoiceAction = $"{xmlHeader}<Response><Redirect>{appHostname}/service/outbound</Redirect></Response>";
                    defaultVoiceAction = $"{xmlHeader}<Response><Redirect>http://{appUrl}:7380/service/outbound</Redirect></Response>";
                    break;
                default:
                    //defaultVoiceAction = $"{xmlHeader}<Response><Redirect>{appHostname}/service/error</Redirect></Response>";
                    defaultVoiceAction = $"{xmlHeader}<Response><Redirect>http://{appUrl}:7380/service/inbound</Redirect></Response>";
                    break;
            }
            }
            responseMessage = Request.CreateResponse(HttpStatusCode.Created, defaultVoiceAction);
            responseMessage.Content = new StringContent(defaultVoiceAction, Encoding.UTF8, "application/xml");
            responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
            return responseMessage;
        }

        [Route("outbound")]
        [HttpPost]

        public HttpResponseMessage outboundHttpResponseMessage([FromForm]  VoiceResponse voiceResponse)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            string defaultVoiceAction = "";
            if (voiceResponse.isActive == "1")
            {
                defaultVoiceAction = sampleOutboundResponse();
                responseMessage = Request.CreateResponse(HttpStatusCode.Created, defaultVoiceAction);
                responseMessage.Content = new StringContent(defaultVoiceAction, Encoding.UTF8, "application/xml");
                responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
                return responseMessage;
            } else {
                // do something else, the call is complete
                return null;

            }
        }

        [Route("inbound")]
        [HttpPost]
       public HttpResponseMessage inboundHttpResponseMessage([FromForm] VoiceResponse voiceResponse)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            string defaultVoiceAction = "";
            Console.WriteLine(voiceResponse);
            if (voiceResponse.isActive == "1")
            {
                defaultVoiceAction = sampleInboundAction();
                responseMessage = Request.CreateResponse(HttpStatusCode.Created, defaultVoiceAction);
                responseMessage.Content = new StringContent(defaultVoiceAction, Encoding.UTF8, "application/xml");
                responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
                return responseMessage;
            } else {
                // do something else, the call is complete
                return null;
            }
        }


        [Route("dtmf")]
        [HttpPost]

        public HttpResponseMessage dtmfHttpResponseMessage([FromForm] DtmfResponse dtmfResponse)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            string defaultVoiceAction = "";
            if (dtmfResponse.dtmfDigits != null)
            {
                defaultVoiceAction = finalDtmf(dtmfResponse.callerNumber);
                responseMessage = Request.CreateResponse(HttpStatusCode.Created, defaultVoiceAction);
                responseMessage.Content = new StringContent(defaultVoiceAction, Encoding.UTF8, "application/xml");
                responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
                return responseMessage;
            } else {
                // do something else, the call is complete
                return null;
            }
        }

        [Route("events")]
        [HttpPost]

        public HttpResponseMessage dtmfHttpResponseMessage([FromForm] VoiceEvents events)
        {
            // Log and save stuff to DB
            return null;
        }
        public string sampleOutboundResponse()
        {
            // You can fetch number to dial or connect to a call from the DB etc 
            // The can be your agents
            // In this case we'll be dialing our SIP agent on a soft-phone
            string appHostname = hostNameResolver();
            //string appHostname = "https://cdb5482a.ngrok.io";
            
            // string mySipNumber = "test.kennedy@ke.sip.africastalking.com";
            //string outboundDialAction = $"{xmlHeader}<Response><Play url=\"{appHostname}/Static/IndianaCut.mp3\" /> </Response>";
            string outboundDialAction = $"{xmlHeader}<Response><Play ur=\"http://{appHostname}:7380/Static/IndianaCut.mp3\" /> </Response>";
            // string outboundDialAction = $"{xmlHeader}<Response><Dial record=\"true\" phoneNumbers=\"{mySipNumber}\" /> </Response>";
            return outboundDialAction;
        }

        
        public string sampleInboundAction()
        {
            string appHostname = hostNameResolver();
            //string appHostname = "https://cdb5482a.ngrok.io";
            string sayActionPrompt = "<Say voice=\"man\"> Please enter your PIN to continue. Press the pound sign to finish</Say>";
            string sayActionTimeout = "<Say voice=\"man\"> Am sorry we did not get that. Good bye</Say>";
            //string getDigitsAction = $"<GetDigits numDigits=\"4\" finishOnKey=\"#\" callbackUrl=\"{appHostname}/service/dtmf\" timeout=\"30\" >{sayActionPrompt}</GetDigits>";
            string getDigitsAction = $"<GetDigits numDigits=\"4\" finishOnKey=\"*\" callbackUrl=\"http://{appHostname}:7380/service/dtmf\" timeout=\"30\" >{sayActionPrompt}</GetDigits>";
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