using System;
using System.Collections;
using System.Text;
using System.Web;
using System.IO;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Linq;

namespace AfricasTalkingCS
{
    public class AfricasTalkingGateway
    {
        private readonly string _username;
        private readonly string _apikey;
        private readonly string _environment;
        private int _responseCode;
        private JsonSerializer _serializer;
        private readonly bool _debug = false;

        public AfricasTalkingGateway(string username,string apikey)
        {
            _username = username;
            _apikey = apikey;
            _environment = "production";
           _serializer =  new JsonSerializer();
        }

        public AfricasTalkingGateway(string username,string apikey,string environment)
        {
            _username = username;
            _apikey = apikey;
            _environment = environment;
            _serializer = new JsonSerializer();
        }

        public dynamic SendMessage(string to, string message, string from = null, int bulkSmsMode = -1, Hashtable options =null)
        {
            //TODO Convert options to type IDictionary
            try
            {
                var data = new Hashtable
                {
                    ["username"] = _username,
                    ["to"] = to,
                    ["message"] = message
                };
                if (from != null)
                {
                    data["from"] = from;
                    data["bulkSmsMode"] = Convert.ToString(bulkSmsMode);
                    if (options != null)
                    {
                        if (options.Contains("keyword"))
                        {
                            data["keyword"] = options["keyword"];
                        }
                        if (options.Contains("linkId"))
                        {
                            data["linkId"] = options["linkId"];
                        }
                        if (options.Contains("enqueue"))
                        {
                            data["enqueue"] = options["enqueue"];
                        }
                        if (options.Contains("retryDurationInHours"))
                            data["retryDurationInHours"] = options["retryDurationInHours"];
                    }
                }
                var response = SendPostRequest(data, SmsUrl);
                dynamic json = JObject.Parse(response);
                    return json;
            }
            catch (AfricasTalkingGatewayException e)
            {
                throw new AfricasTalkingGatewayException(e);
            }
        }

        public void UploadMediaFile(string url)
        {
            var data = new Hashtable
            {
                ["username"] = _username,
                ["url"] = url
            };
            var urlString = VoiceUrl + "/mediaUpload";
            var response = SendPostRequest(data, urlString);
            dynamic json = JObject.Parse(response);
            if ((string)json["errorMesage"] != "None")
            {
                throw  new  AfricasTalkingGatewayException(json["errorMessage"]);
            }
        }

        public dynamic FetchMessages(int lastReceivedId)
        {
            var url = SmsUrl + "?username=" + _username + "&lastReceivedId" + Convert.ToString(lastReceivedId);
            var response = SendGetRequest(url);

            if (_responseCode != (int) HttpStatusCode.OK) throw new AfricasTalkingGatewayException(response);
            dynamic json = JObject.Parse(response);
            return json["SMSMessageData"]["Messages"];
        }

        public dynamic CreateSubscription(string phoneNumber, string shortCode, string keyWord, string checkoutToken)
        {
            if (phoneNumber.Length == 0 || shortCode.Length == 0 || keyWord.Length == 0 || checkoutToken.Length == 0)
            {
                throw new AfricasTalkingGatewayException("Some Parameters are missing!");

            }
            var data = new Hashtable
            {
                ["username"] = _username,
                ["phoneNumber"] = phoneNumber,
                ["shortCode"] = shortCode,
                ["keyword"] = keyWord,
                ["checkoutToken"] = checkoutToken
            };
            var url = SubscriptionUrl + "/create";
            var response = SendPostRequest(data, url);
            if (_responseCode != (int) HttpStatusCode.Created) throw new AfricasTalkingGatewayException(response);
            dynamic json = JObject.Parse(response);
            return json;
        }

        public dynamic DeleteSubscription(string phoneNumber, string shortCode, string keyWord)
        {
            if (phoneNumber.Length == 0 || shortCode.Length == 0 || keyWord.Length == 0)
            {
                throw new AfricasTalkingGatewayException("Some Parameters are missing!");
            }
            var data = new Hashtable
            {
                ["username"] = _username,
                ["phoneNumber"] = phoneNumber,
                ["shortCode"] = shortCode,
                ["keyword"] = keyWord
            };
            var url = SubscriptionUrl + "/delete";
            var response = SendPostRequest(data, url);
            if (_responseCode != (int) HttpStatusCode.Created) throw new AfricasTalkingGatewayException(response);
            dynamic json = JObject.Parse(response);
            return json;
        }

        public dynamic Call(string from, string to)
        {
            var data = new Hashtable
            {
                ["username"] = _username,
                ["from"] = from,
                ["to"] = to
            };

            // var data = new Dictionary<string, string>
            // {
            //    { "username", _username },
            //    { "from", from },
            //    { "to", to }
            // };
            
            try
            {
                var url = VoiceUrl + "/call";
                var response = SendPostRequest(data, url);
                dynamic json = JObject.Parse(response);
                return json;
            }
            catch (AfricasTalkingGatewayException e)
            {
                throw new AfricasTalkingGatewayException(e);
            }
        }

        public int GetNumberOfQueuedCalls(string phoneNumber, string queueName = null)
        {
            var data = new Hashtable
            {
                ["username"] = _username,
                ["phoneNumbers"] = phoneNumber,
            };
            if (queueName != null)
            {
                data["queueName"] = queueName;
            }
            var url = VoiceUrl + "/queueStatus";
            var response = SendPostRequest(data, url);
            dynamic json = JObject.Parse(response);
            if ((string)json["errorMessage"] == "None")
            {
                return json["entries"];
            }
            throw new AfricasTalkingGatewayException(json["errorMessage"]);
        }

        public dynamic SendAirtime(dynamic recepients)
        {
            var urlString = AirtimeUrl + "/send";
            var recipients = JObject.Parse(recepients);
            var data = new Hashtable { ["username"] = _username, ["recipients"] = "[" + recipients + "]" };
            try
            {
                var response = SendPostRequest(data, urlString);
                if (_responseCode != (int)HttpStatusCode.Created) throw new AfricasTalkingGatewayException(response);
                dynamic json = JObject.Parse(response);
                return json;
            }
            catch (AfricasTalkingGatewayException e)
            {
                throw new AfricasTalkingGatewayException(e);
            }
        }

        public dynamic GetUserData()
        {
            var urlString = Userdata + "?username=" + _username;
            var response = SendGetRequest(urlString);
            if (_responseCode != (int) HttpStatusCode.OK) throw new AfricasTalkingGatewayException(response);
            dynamic json = JObject.Parse(response);
            return json;
        }

        /// <summary>
        /// Main payments endpoint.
        /// </summary>
        private string PaymentsUrl => PaymentsHost + "/mobile/checkout/request";

        /// <summary>
        /// Business to Business API endpoint.
        /// </summary>
        private string B2BPaymentsUrl => PaymentsHost + "/mobile/b2b/request";

        /// <summary>
        /// Business to Client Endpoint.
        /// </summary>
        private string B2CPaymentsUrl => PaymentsHost + "/mobile/b2c/request";

        /// <summary>
        /// Subscription endpoint.
        /// </summary>
        private string SubscriptionUrl => ApiHost + "/version1/subscription";

        /// <summary>
        /// The Userdata endpoint.
        /// </summary>
        private string Userdata => ApiHost + "/version1/user";

        /// <summary>
        /// Airtime Endpoint.
        /// </summary>
        private string AirtimeUrl => ApiHost + "/version1/airtime";

        /// <summary>
        /// Voice endpoint.
        /// </summary>
        private string VoiceUrl => (ReferenceEquals(_environment, "sandbox")
            ? "https://voice.sandbox.africastalking.com"
            : "https://voice.africastalking.com");

        /// <summary>
        /// SMS Endpoint.
        /// </summary>
        private string SmsUrl => ApiHost + "/version1/messaging";

        /// <summary>
        /// Root API host.
        /// </summary>
        private string ApiHost => (ReferenceEquals(_environment,"sandbox")? "https://api.sandbox.africastalking.com": "https://api.africastalking.com");

        /// <summary>
        /// Payment endpoint.
        /// </summary>
        private string PaymentsHost => (ReferenceEquals(_environment, "sandbox") ? "https://payments.sandbox.africastalking.com" : "https://payments.africastalking.com");

        private string SendGetRequest(string urlString)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;
                var webRequest = (HttpWebRequest)WebRequest.Create(urlString);
                webRequest.Method = "GET";
                webRequest.Accept = "application/json";
                webRequest.Headers.Add("apiKey", _apikey);
                var httpResponse = (HttpWebResponse)webRequest.GetResponse();
                _responseCode = (int)httpResponse.StatusCode;
                var webpageReader = new StreamReader(httpResponse.GetResponseStream() ?? throw new InvalidOperationException());
                var response = webpageReader.ReadToEnd();
                if (_debug)
                    Console.WriteLine("Full response: " + response);
                return response;

            }

            catch (WebException ex)
            {
                if (ex.Response == null) throw new AfricasTalkingGatewayException(ex.Message);
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
                {
                    var response = reader.ReadToEnd();

                    if (_debug) Console.WriteLine("Full response: " + response);
                    return response;
                }
            }
        }

        private string SendPostRequest(IDictionary data, string urlString)
        {
            try
            {
                var dataStr = "";
                foreach (string key in data.Keys)
                {
                    if (dataStr.Length > 0)
                    {
                        dataStr += "&";
                    }

                    //var value = (string)data[key];
                    var value = data[key].ToString();
                    dataStr += HttpUtility.UrlEncode(key, Encoding.UTF8);
                    dataStr += "=" + HttpUtility.UrlEncode(value, Encoding.UTF8);
                }

                var byteArray = Encoding.UTF8.GetBytes(dataStr);
                ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;
                var webRequest = (HttpWebRequest)WebRequest.Create(urlString);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = byteArray.Length;
                webRequest.Accept = "application/json";
                webRequest.Headers.Add("apiKey", _apikey);

                var webpageStream = webRequest.GetRequestStream();
                webpageStream.Write(byteArray, 0, byteArray.Length);
                webpageStream.Close();

                var httpResponse = (HttpWebResponse)webRequest.GetResponse();
                _responseCode = (int)httpResponse.StatusCode;
                var webpageReader = new StreamReader(
                    httpResponse.GetResponseStream() ?? throw new InvalidOperationException());
                var response = webpageReader.ReadToEnd();

                if (_debug) Console.WriteLine("Response: " + response);

                return response;
            }
            catch (WebException webException)
            {
                if (webException.Response == null)
                {
                    throw new AfricasTalkingGatewayException(webException.Message);
                }

                using (var stream = webException.Response.GetResponseStream())
                using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
                {
                    var response = reader.ReadToEnd();
                    if (_debug)
                    {
                        Console.WriteLine("Exception: " + response);
                    }

                    return response;
                }
            }
        }

        /// <summary>
        /// The checkout.
        /// </summary>
        /// <param name="productName">
        /// The product name.
        /// </param>
        /// <param name="phoneNumber">
        /// The phone number for example +254....
        /// </param>
        /// <param name="currencyCode">
        /// The currency code for example KES, UGX
        /// </param>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <param name="providerChannel">
        /// The provider channel.
        /// </param>
        /// <returns>
        /// Returns transaction status
        /// </returns>
        /// <exception cref="AfricasTalkingGatewayException">
        ///  Throws an error of type 40X 
        /// </exception>
        public dynamic Checkout(string productName, string phoneNumber , string currencyCode, int amount, string providerChannel ,Dictionary<string,string> metadata = null)
        {
            var checkout = new CheckOutData
            {
                username = _username,
                productName = productName,
                phoneNumber = phoneNumber,
                currencyCode = currencyCode,
                amount = amount,
                providerChannel = providerChannel
            };
            if (metadata != null)
            {
                checkout.metadata = metadata;
            }

            try
            {
                var checkoutResponse = PostAsJson(checkout, PaymentsUrl);
                return checkoutResponse;
            }
            catch (Exception e)
            {
                throw new AfricasTalkingGatewayException(e.StackTrace+e.Message+e.Source);
            }
        }

        /// <summary>
        /// This method handles POST requests to the POST request the B2B endpoint
        /// </summary>
        /// <param name="dataMap">
        /// Strunctured JSON Object conataining all B2B arguments.
        /// </param>
        /// <param name="url">
        /// The B2B End-point.
        /// </param>
        /// <returns>
        /// Server response, stringified.
        /// </returns>
        private string PostB2BJson(B2BData dataMap, string url)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("apiKey", this._apikey);
            var result = client.PostAsJsonAsync(url, dataMap).Result;
            result.EnsureSuccessStatusCode();
            var stringResult = result.Content.ReadAsStringAsync().Result;
            return stringResult;
        }

        public dynamic MobileB2B(string product, string providerChannel, string transfer, string currency,
            decimal transferAmount, string channelReceiving, string accountReceiving,dynamic b2Bmetadata)
        {
            var bTob = new B2BData
            {
                username = _username,
                productName = product,
                provider = providerChannel,
                transferType = transfer,
                currencyCode = currency,
                amount = transferAmount,
                destinationAccount = accountReceiving,
                destinationChannel = channelReceiving,
                metadata = b2Bmetadata
            };
            try
            {
                Console.WriteLine(bTob);
                var response = PostB2BJson(bTob, B2BPaymentsUrl);
                return response;
            }
            catch (Exception e)
            {
                throw new AfricasTalkingGatewayException(e);
            }
        }
        
        public dynamic MobileB2C(string productName, IEnumerable<MobileB2CRecepient> recepients)
        {
            var requestBody = new RequestBody
            {
                productName = productName,
                username = _username,
                recepients = recepients.ToList()
            };
            var response = Post(requestBody,B2CPaymentsUrl);
            return response;
        }


        private DataResult Post(RequestBody requestBody, string url)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("apikey", _apikey);
             var res = httpClient.PostAsJsonAsync(url, requestBody).Result;
            res.EnsureSuccessStatusCode();
            var result = res.Content.ReadAsAsync<DataResult>();
            return result.Result;
        }

        private string PostAsJson(CheckOutData dataMap, string url)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("apiKey", _apikey);
            var result = client.PostAsJsonAsync(url, dataMap).Result;
            result.EnsureSuccessStatusCode();
            var stringResult = result.Content.ReadAsStringAsync().Result;
            return stringResult;

        }
        private static bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            return true;
        }
    }
}
