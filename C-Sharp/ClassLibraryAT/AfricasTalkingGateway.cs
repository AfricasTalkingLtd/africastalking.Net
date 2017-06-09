using System;
using System.Collections;
using System.Text;
using System.Web;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

//Note add System.Web.Script.Serialization, using System.Net.Http, Newtonsoft.Json, sytem.Net.Http.Formatting extension reference to the sln
public class AfricasTalkingGatewayException : Exception
{
    public AfricasTalkingGatewayException(string message)
            : base(message) { }
    public AfricasTalkingGatewayException(Exception ex) : base(ex.Message, ex)
    {

    }
}
public class AfricasTalkingGateway
{
    private string _username;
    private string _apiKey;
    private string _environment;
    private int responseCode;
    private JavaScriptSerializer serializer;

    //Change the debug flag to true to view the full response
    private Boolean DEBUG = false;

    public AfricasTalkingGateway(string username_, string apiKey_)
    {
        _username = username_;
        _apiKey = apiKey_;
        _environment = "production";
        serializer = new JavaScriptSerializer();
    }

    public AfricasTalkingGateway(string username, string apiKey, string environment)
    {
        _username = username;
        _apiKey = apiKey;
        _environment = environment;
        serializer = new JavaScriptSerializer();
    }

    public dynamic sendMessage (string to_, string message_, string from_ = null, int bulkSMSMode_ = 1, Hashtable options_ = null)
    {       
        Hashtable data = new Hashtable ();
        data ["username"] = _username;
        data ["to"] = to_;
        data ["message"] = message_;

        if (from_ != null) {
            data ["from"] = from_;
            data ["bulkSMSMode"] = Convert.ToString (bulkSMSMode_);

            if (options_ != null) {
                if (options_.Contains("keyword")) {
                    data["keyword"] = options_["keyword"];
                }

                if (options_.Contains("linkId")) {
                    data["linkId"] = options_["linkId"];
                }

                if (options_.Contains("enqueue")) {
                    data["enqueue"] = options_["enqueue"];
                }

                if(options_.Contains("retryDurationInHours"))
                    data["retryDurationInHours"] = options_["retryDurationInHours"];
            }
        }

        string response = sendPostRequest (data, SMS_URLString);
        if (responseCode == (int)HttpStatusCode.Created) {
            var json = serializer.Deserialize <dynamic> (response);
            dynamic recipients = json ["SMSMessageData"] ["Recipients"];
            if(recipients.Length > 0) {
                return recipients;
            }
            throw new AfricasTalkingGatewayException(json ["SMSMessageData"] ["Message"]);
        }
        throw new AfricasTalkingGatewayException(response);
    }
    public dynamic fetchMessages(int lastReceivedId_)
    {
        string url = SMS_URLString + "?username=" + _username + "&lastReceivedId=" + Convert.ToString(lastReceivedId_);
        string response = sendGetRequest (url);
        if (responseCode == (int)HttpStatusCode.OK) {
            dynamic json = serializer.DeserializeObject (response);
            return json ["SMSMessageData"] ["Messages"];
        }
        throw new AfricasTalkingGatewayException (response);
    }
    public dynamic createSubscription(string phoneNumber_, string shortCode_, string keyword_) 
    {
        if(phoneNumber_.Length == 0 || shortCode_.Length == 0 || keyword_.Length == 0)
            throw new AfricasTalkingGatewayException("Please supply phone number, short code and keyword");
        Hashtable data_ = new Hashtable ();
        data_ ["username"] = _username;
        data_ ["phoneNumber"] = phoneNumber_;
        data_ ["shortCode"] = shortCode_;
        data_ ["keyword"] = keyword_;
        string urlString = SUBSCRIPTION_URLString + "/create";
        string response = sendPostRequest (data_, urlString);
        if (responseCode == (int)HttpStatusCode.Created) {
            dynamic json = serializer.Deserialize<dynamic> (response);
            return json;
        }
        throw new AfricasTalkingGatewayException (response);
    }    
    public dynamic deleteSubscription(string phoneNumber_, string shortCode_, string keyword_) 
    {
        if(phoneNumber_.Length == 0 || shortCode_.Length == 0 || keyword_.Length == 0)
            throw new AfricasTalkingGatewayException("Please supply phone number, short code and keyword");
        Hashtable data_ = new Hashtable ();
        data_ ["username"] = _username;
        data_ ["phoneNumber"] = phoneNumber_;
        data_ ["shortCode"] = shortCode_;
        data_ ["keyword"] = keyword_;
        string urlString = SUBSCRIPTION_URLString + "/delete";
        string response = sendPostRequest (data_, urlString);
        if (responseCode == (int)HttpStatusCode.Created) {
            dynamic json = serializer.Deserialize<dynamic> (response);
            return json;
        }
        throw new AfricasTalkingGatewayException (response);
    }    
    public dynamic call (string from_, string to_)
    {
        Hashtable data = new Hashtable ();
        data ["username"] = _username;
        data ["from"] = from_;
        data ["to"] = to_;
        string urlString = VOICE_URLString + "/call";
        string response = sendPostRequest (data, urlString);
        dynamic json = serializer.Deserialize<dynamic> (response);
        if ((string)json ["errorMessage"] == "None") {
            return json["entries"];
        }
        throw new AfricasTalkingGatewayException (json ["errorMessage"]);
    }
    public int getNumQueuedCalls(string phoneNumber_, string queueName_ = null)
    {
        Hashtable data = new Hashtable ();
        data ["username"] = _username;
        data ["phoneNumbers"] = phoneNumber_;
        if (queueName_ != null)
            data ["queueName"] = queueName_;

        string urlString = VOICE_URLString + "/queueStatus";
        string response = sendPostRequest (data, urlString);
        dynamic json = serializer.Deserialize<dynamic> (response);
        if ((string)json["errorMessage"] == "None") {
            return json["entries"];
        }
        throw new AfricasTalkingGatewayException (json["errorMessage"]);
    }
    public void uploadMediaFile(string url_) {
        Hashtable data = new Hashtable ();
        data ["username"] = _username;
        data ["url"] = url_;

        string urlString = VOICE_URLString + "/mediaUpload";
        string response = sendPostRequest (data, urlString);
        dynamic json = serializer.Deserialize<dynamic> (response);
        if((string)json["errorMessage"] != "None")
            throw new AfricasTalkingGatewayException (json["errorMessage"]);
    }    
    public dynamic sendAirtime(ArrayList recipients_) 
    {
        string urlString = AIRTIME_URLString + "/send";
        string recipients = new JavaScriptSerializer ().Serialize (recipients_);
        Hashtable data = new Hashtable ();
        data ["username"] = _username;
        data ["recipients"] = recipients;
        string response = sendPostRequest (data, urlString);
        if (responseCode == (int)HttpStatusCode.Created) {
            dynamic json = serializer.Deserialize<dynamic> (response);
            if (json ["responses"].Count > 0)
                return json ["responses"];
            throw new AfricasTalkingGatewayException (json ["errorMessage"]);
        }
        throw new AfricasTalkingGatewayException (response);
    }
    public dynamic getUserData()
    {
        string urlString = USERDATA_URLString + "?username=" + _username;
        string response = sendGetRequest (urlString);
        if (responseCode == (int)HttpStatusCode.OK) {
            dynamic json = serializer.Deserialize<dynamic> (response);
            return json ["UserData"];
        }
        throw new AfricasTalkingGatewayException (response);
    }

    private string sendPostRequest (Hashtable dataMap_, string urlString_)
    {
        try {
            string dataStr  = "";
            foreach (string key in dataMap_.Keys) {
                if (dataStr.Length > 0 ) dataStr += "&";
                string value = (string)dataMap_[key];
                dataStr += HttpUtility.UrlEncode (key, Encoding.UTF8);
                dataStr += "=" + HttpUtility.UrlEncode (value, Encoding.UTF8);
            }

            byte[] byteArray = Encoding.UTF8.GetBytes (dataStr);

            System.Net.ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(urlString_);

            webRequest.Method        = "POST";
            webRequest.ContentType   = "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;
            webRequest.Accept        = "application/json";

            webRequest.Headers.Add ("apiKey", _apiKey);    

            Stream webpageStream = webRequest.GetRequestStream ();
            webpageStream.Write (byteArray, 0, byteArray.Length);
            webpageStream.Close ();

            HttpWebResponse httpResponse = (HttpWebResponse) webRequest.GetResponse ();
            responseCode = (int)httpResponse.StatusCode;
            StreamReader webpageReader = new StreamReader (httpResponse.GetResponseStream ());
            string response = webpageReader.ReadToEnd();

            if(DEBUG)
                Console.WriteLine("Full response: " + response);

            return response;

        } catch (WebException ex) {
            if(ex.Response == null)
                throw new AfricasTalkingGatewayException(ex.Message);
            using (var stream = ex.Response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                string response = reader.ReadToEnd ();

                if (DEBUG)
                    Console.WriteLine ("Full response: " + response);

                        return response;
            }
        }

        catch(AfricasTalkingGatewayException ex) {
            throw ex;
        }
    }
    private string sendGetRequest (string urlString_)
    {
        try{
            System.Net.ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(urlString_);
            webRequest.Method = "GET";
            webRequest.Accept = "application/json";
            webRequest.Headers.Add ("apiKey", _apiKey);   

            HttpWebResponse httpResponse = (HttpWebResponse)webRequest.GetResponse ();
            responseCode = (int)httpResponse.StatusCode;
            StreamReader webpageReader = new StreamReader (httpResponse.GetResponseStream ());

            string response = webpageReader.ReadToEnd();

            if(DEBUG)
                Console.WriteLine("Full response: " + response);

            return response;

        }

        catch (WebException ex) {
            if(ex.Response == null)
                throw new AfricasTalkingGatewayException(ex.Message);

            using (var stream = ex.Response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                string response = reader.ReadToEnd ();

                if (DEBUG)
                    Console.WriteLine ("Full response: " + response);

                return response;
            }
        }

        catch(AfricasTalkingGatewayException ex) {
            throw ex;
        }
    }
    public string PostAsJson(CheckOutData dataMap, string url)
    {
        var client = new HttpClient();

        client.DefaultRequestHeaders.Add("apiKey", _apiKey);
        var result = client.PostAsJsonAsync<CheckOutData>(url, dataMap).Result;
        result.EnsureSuccessStatusCode();

        var stringResult = result.Content.ReadAsStringAsync().Result;
        return stringResult;

    }    
    public class CheckOutData
    {
        public string username { get; set; }
        public string productName { get; set; }
        public string phoneNumber { get; set; }
        public string currencyCode { get; set; }
        public decimal amount { get; set; }
        public string providerChannel { get; set; }
    }
    public dynamic initiateMobilePaymentCheckout(string productName_, string phoneNumber_, string currencyCode_, int amount_, string providerChannel_)
    {
        var CheckOutData = new CheckOutData()
        {
            username = _username,
            productName = productName_,
            phoneNumber = phoneNumber_,
            currencyCode = currencyCode_,
            amount = amount_,
            providerChannel = providerChannel_,
        };

        try
        {
            string checkOutresponse = PostAsJson(CheckOutData, PAYMENTS_URLString);
            return checkOutresponse;
        }
        catch (Exception ex)
        {
            throw new AfricasTalkingGatewayException(ex);
        }
    }
   public string PostB2BJson(B2BData dataMap, string url)
    {
        var client = new HttpClient();

        client.DefaultRequestHeaders.Add("apiKey", _apiKey);
        var result = client.PostAsJsonAsync<B2BData>(url, dataMap).Result;
        result.EnsureSuccessStatusCode();

        var stringResult = result.Content.ReadAsStringAsync().Result;
        return stringResult;

    }
    public class B2BData
    {
        public string Username { get; set; }
        public string ProductName { get; set; }
        public string Provider { get; set; }
        public string TransferType { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
        public string DestinationChannel { get; set; }
        public string DestinationAccount { get; set; }
    }
    public dynamic MobileB2B(string productName, string provider, string transferType, string currencyCode, int amount, string destinationChannel, string destinationAccount)
    {
        var b2BData = new B2BData()
        {
            Username = _username,
            ProductName = productName,
            Provider = provider,
            TransferType = transferType,
            CurrencyCode = currencyCode,
            Amount = amount,
            DestinationChannel = destinationChannel,
            DestinationAccount = destinationAccount
        };

        try
        {
            string b2Bresponse = PostB2BJson(b2BData, PAYMENTS_B2B_URLString);
            return b2Bresponse;
        }
        catch (Exception ex)
        {
            throw new AfricasTalkingGatewayException(ex);
        }
    }  
    public dynamic MobilePaymentB2CRequest(string productName, IList<MobilePaymentB2CRecipient> recipients)
    {
        var requestBody = new RequestBody
        {
            ProductName = productName,
            UserName = _username,
            Recipients = recipients.ToList()
        };        
        Console.WriteLine("Raw Request: " + requestBody);
        var response = Post(requestBody, this.PaymentsB2CUrlString);
        return response;
    }
    public DataResult Post(RequestBody body, string url)
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("apiKey", _apiKey);
        var result = httpClient.PostAsJsonAsync(url, body).Result;
        result.EnsureSuccessStatusCode();
        var res = result.Content.ReadAsAsync<DataResult>();
        return res.Result;

    }
    private bool RemoteCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors errors)
    {
        return true;
    }
    private string ApiHost
    {
        get
        {
            return (string.ReferenceEquals(_environment, "sandbox") ? "https://api.sandbox.africastalking.com" : "https://api.africastalking.com");
        }
    }
    private string PaymentHost
    {
        get
        {
            return (string.ReferenceEquals(_environment, "sandbox") ? "https://payments.sandbox.africastalking.com" : "https://payments.africastalking.com");
        }

    }
    private string SMS_URLString
    {
        get
        {
            return ApiHost + "/version1/messaging";
        }
    }
    private string VOICE_URLString
    {
        get
        {
            return (string.ReferenceEquals(_environment, "sandbox") ? "https://voice.sandbox.africastalking.com" : "https://voice.africastalking.com");
        }
    }

    private string SUBSCRIPTION_URLString
    {
        get
        {
            return ApiHost + "/version1/subscription";
        }
    }

    private string USERDATA_URLString
    {
        get
        {
            return ApiHost + "/version1/user";
        }
    }
    private string AIRTIME_URLString
    {
        get
        {
            return ApiHost + "/version1/airtime";
        }
    }
    private string PAYMENTS_URLString
    {
        get
        {
            return PaymentHost + "/mobile/checkout/request";
        }
    }
    private string PAYMENTS_B2B_URLString
    {
        get
        {
            return PaymentHost + "/mobile/b2b/request";
        }
    }
    private string PaymentsB2CUrlString
    {
        get
        {
            return PaymentHost + "/mobile/b2c/request";
        }
    }

}
public class MobilePaymentB2CRecipient
{
    [JsonProperty("phoneNumber")]
    public string PhoneNumber { get; set; }
    [JsonProperty("currencyCode")]
    public string CurrencyCode { get; set; }
    [JsonProperty("amount")]
    public decimal Amount { get; set; }
    [JsonProperty("metadata")]
    public Dictionary<string, string> Metadata { get; private set; }
    public MobilePaymentB2CRecipient(string phoneNumber, string currencyCode, decimal amount)
    {
        PhoneNumber = phoneNumber;
        CurrencyCode = currencyCode;
        Amount = amount;
        Metadata = new Dictionary<string, string>();
    }
    public void AddMetadata(string key, string value)
    {
        this.Metadata.Add(key, value);
    }
    public string ToJson()
    {
        var json = JsonConvert.SerializeObject(this);
        return json;
    }
}
public class RequestBody
{
    public RequestBody()
    {
        this.Recipients = new List<MobilePaymentB2CRecipient>();
    }
    [JsonProperty("username")]
    public string UserName { get; set; }
    [JsonProperty("productName")]
    public string ProductName { get; set; }
    [JsonProperty("recipients")]
    public List<MobilePaymentB2CRecipient> Recipients { get; set; }
    public override string ToString()
    {
        var json = JsonConvert.SerializeObject(this);
        return json;
    }
}
public class Entry
{

    [JsonProperty("phoneNumber")]
    public string phoneNumber { get; set; }

    [JsonProperty("provider")]
    public string provider { get; set; }

    [JsonProperty("providerChannel")]
    public string providerChannel { get; set; }

    [JsonProperty("transactionFee")]
    public string transactionFee { get; set; }

    [JsonProperty("status")]
    public string status { get; set; }

    [JsonProperty("value")]
    public string value { get; set; }

    [JsonProperty("transactionId")]
    public string transactionId { get; set; }
}
public class DataResult
{
    [JsonProperty("numQueued")]
    public int numQueued { get; set; }

    [JsonProperty("entries")]
    public IList<Entry> entries { get; set; }

    [JsonProperty("totalValue")]
    public string totalValue { get; set; }

    [JsonProperty("totalTransactionFee")]
    public string totalTransactionFee { get; set; }

    public override string ToString()
    {
        var result = JsonConvert.SerializeObject(this);
        return result;
    }
}


