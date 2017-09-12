using System;
using System.Collections;
using System.Text;
using System.Web;
using System.IO;
using System.Net;
//using System.Web.Script.Serialization;
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
           
            Hashtable data = new Hashtable
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
                    if ((string)json["errorMessage"] == "None")
                    {
                        return json;
                    }
                    
            throw new AfricasTalkingGatewayException(response);
                
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

        public dynamic CreateSubscription(string phoneNumber, string shortCode, string keyWord)
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
            Hashtable data = new Hashtable
            {
                ["username"] = _username,
                ["from"] = from,
                ["to"] = to
            };

            var url = VoiceUrl + "/call";
            var response = SendPostRequest(data, url);
            dynamic json = JObject.Parse(response);
            if ((string)json["errorMessage"] == "None")
            {
                return json["entries"];
            }
            throw  new AfricasTalkingGatewayException(json["errorMessage"]);

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

        public dynamic SendAirtime(string recepients)
        {
            var urlString = AirtimeUrl + "/send";
            var recipients = JObject.Parse(recepients);
            var data = new Hashtable
            {
                ["username"] = _username,
                ["recipients"] = recipients
            };
            var response = SendPostRequest(data, urlString);
            if (_responseCode != (int) HttpStatusCode.Created) throw new AfricasTalkingGatewayException(response);
            dynamic json = JObject.Parse(response);
            if (json["responses"] != null)
            {
                return json["responses"];
            }
                
            throw new AfricasTalkingGatewayException(json["errorMessage"]);
        }

        public dynamic GetUserData()
        {
            var urlString = Userdata + "?username=" + _username;
            var response = SendGetRequest(urlString);
            if (_responseCode != (int) HttpStatusCode.OK) throw new AfricasTalkingGatewayException(response);
            dynamic json = JObject.Parse(response);
            return json["UserData"];
        }
        
        private string PaymentsUrl => PaymentsHost + "/mobile/checkout/request";

        private string B2BPaymentsUrl => PaymentsHost + "/mobile/b2b/request"; 

        private string SubscriptionUrl => ApiHost + "/version1/subscription";

        private string Userdata => ApiHost + "/version1/user";

        private string AirtimeUrl => ApiHost + "/version1/airtime";

        private string VoiceUrl => (ReferenceEquals(_environment, "sandbox")
            ? "https://voice.sandbox.africastalking.com"
            : "https://voice.africastalking.com");

        private string SmsUrl => ApiHost + "/version1/messaging";

        private string ApiHost => (ReferenceEquals(_environment,"sandbox")? "https://api.sandbox.africastalking.com": "https://api.africastalking.com");

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
                if (ex.Response == null)
                    throw new AfricasTalkingGatewayException(ex.Message);

                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
                {
                    var response = reader.ReadToEnd();

                    if (_debug)
                        Console.WriteLine("Full response: " + response);

                    return response;
                }
            }
        }

        private string SendPostRequest(Hashtable data, string urlString)
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
                    var value = (string) data[key];
                    dataStr += HttpUtility.UrlEncode(key, Encoding.UTF8);
                    dataStr += "=" + HttpUtility.UrlEncode(value, Encoding.UTF8);

                }
                var byteArray = Encoding.UTF8.GetBytes(dataStr);

                ServicePointManager.ServerCertificateValidationCallback =
                    RemoteCertificateValidationCallback;
                var webRequest = (HttpWebRequest) WebRequest.Create(urlString);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = byteArray.Length;
                webRequest.Accept = "application/json";
                webRequest.Headers.Add("apiKey", _apikey);

                var webpageStream = webRequest.GetRequestStream();
                webpageStream.Write(byteArray, 0, byteArray.Length);
                webpageStream.Close();

                var httpResponse = (HttpWebResponse) webRequest.GetResponse();
                _responseCode = (int) httpResponse.StatusCode;
                var webpageReader = new StreamReader(httpResponse.GetResponseStream() ?? throw new InvalidOperationException());
                var response = webpageReader.ReadToEnd();

                if (_debug)
                    Console.WriteLine("Response: " + response);

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

        public dynamic Checkout(string productName, string phoneNumber , string currencyCode, int amount, string providerChannel)
        {
            var checkout = new CheckOutData
            {
                Username = _username,
                ProductName = productName,
                PhoneNumber = phoneNumber,
                CurrencyCode = currencyCode,
                Amount = amount,
                ProviderChannel = providerChannel
            };
            try
            {
                var checkoutResponse = PostAsJson(checkout, PaymentsUrl);
                return checkoutResponse;
            }
            catch (Exception e)
            {
                throw new AfricasTalkingGatewayException(e);
            }
        }

        private string PostB2BJson(B2BData dataMap, string url)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("apiKey", _apikey);
            var result = client.PostAsJsonAsync(url, dataMap).Result;
            result.EnsureSuccessStatusCode();
            var stringResult = result.Content.ReadAsStringAsync().Result;
            return stringResult;
        }

        public dynamic MobileB2B(string productName, string provider, string transferType, string currencyCode,
            int amount, string destinationChannel, string destinationAccount)
        {
            var bTob = new B2BData
            {
                Username = _username,
                ProductName = productName,
                ProviderChannel = provider,
                TransferType = transferType,
                CurrencyCode = currencyCode,
                Amount = amount,
                DestinationChannel = destinationChannel,
                DestinationAccount = destinationAccount
            };

            try
            {
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
                ProductName = productName,
                UserName = _username,
                Recepients = recepients.ToList()
            };
            Console.WriteLine("Request Body: "+requestBody);
            var response = Post(requestBody, B2BPaymentsUrl);
            return response;
        }

        private DataResult Post(RequestBody requestBody, string b2BPaymentsUrl)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("apiKey",_apikey);
            var res = httpClient.PostAsJsonAsync(b2BPaymentsUrl, requestBody).Result;
            res.EnsureSuccessStatusCode();
            var result = res.Content.ReadAsAsync<DataResult>();
             return  result.Result;
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
        private bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            return true;
        }
    }
}
