// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AfricasTalkingGateway.cs" company="Africa's Talking">
//   2017
// </copyright>
// <summary>
//   Defines the AfricasTalkingGateway type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AfricasTalkingCS
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The africas talking gateway class. Accepting sandbox as an environment
    /// </summary>
    public class AfricasTalkingGateway
    {
        private readonly string _username;
        private readonly string _apikey;
        private readonly string _environment;
        private int _responseCode;
        private JsonSerializer _serializer;
        private readonly bool _debug = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="AfricasTalkingGateway"/> class.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="apikey">
        /// The apikey.
        /// </param>
        public AfricasTalkingGateway(string username,string apikey)
        {
            _username = username;
            _apikey = apikey;
            _environment = "production";
            _serializer = new JsonSerializer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AfricasTalkingGateway"/> class.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="apikey">
        /// The apikey.
        /// </param>
        /// <param name="environment">
        /// The environment.
        /// </param>
        public AfricasTalkingGateway(string username,string apikey,string environment)
        {
            _username = username;
            _apikey = apikey;
            _environment = environment;
            _serializer = new JsonSerializer();
        }

        /// <summary>
        /// The send message method.
        /// </summary>
        /// <param name="to">
        /// The Receipient(s).
        /// </param>
        /// <param name="message">
        /// The message content.
        /// </param>
        /// <param name="from">
        /// The Sender.
        /// </param>
        /// <param name="bulkSmsMode">
        /// The bulk sms mode: set 1 for Bulk SMS
        /// </param>
        /// <param name="options">
        /// The Options for premium sms.
        /// </param>
        /// <returns>
        /// The <see cref="dynamic"/>.
        /// </returns>
        /// <exception cref="AfricasTalkingGatewayException">
        /// </exception>
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

        /// <summary>
        /// The upload media file method.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <exception cref="AfricasTalkingGatewayException">
        /// </exception>
        public void UploadMediaFile(string url)
        {
            var data = new Hashtable
            {
                ["username"] = _username,
                ["url"] = url
            };
            var urlString = this.VoiceUrl + "/mediaUpload";
            var response = SendPostRequest(data, urlString);
            dynamic json = JObject.Parse(response);
            if ((string)json["errorMesage"] != "None")
            {
                throw new AfricasTalkingGatewayException(json["errorMessage"]);
            }
        }

        /// <summary>
        /// The fetch messages method.
        /// </summary>
        /// <param name="lastReceivedId">
        /// The last received id.
        /// </param>
        /// <returns>
        /// The <see cref="dynamic"/>.
        /// </returns>
        /// <exception cref="AfricasTalkingGatewayException">
        /// </exception>
        public dynamic FetchMessages(int lastReceivedId)
        {
            var url = SmsUrl + "?username=" + _username + "&lastReceivedId" + Convert.ToString(lastReceivedId);
            var response = SendGetRequest(url);

            if (_responseCode != (int) HttpStatusCode.OK) throw new AfricasTalkingGatewayException(response);
            dynamic json = JObject.Parse(response);
            return json["SMSMessageData"]["Messages"];
        }

        /// <summary>
        /// The create subscription method.
        /// </summary>
        /// <param name="phoneNumber">
        /// The phone number.
        /// </param>
        /// <param name="shortCode">
        /// The short code.
        /// </param>
        /// <param name="keyWord">
        /// The key word.
        /// </param>
        /// <param name="checkoutToken">
        /// The checkout token.
        /// </param>
        /// <returns>
        /// The <see cref="dynamic"/>.
        /// </returns>
        /// <exception cref="AfricasTalkingGatewayException">
        /// </exception>
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

        /// <summary>
        /// The delete subscription method.
        /// </summary>
        /// <param name="phoneNumber">
        /// The phone number.
        /// </param>
        /// <param name="shortCode">
        /// The short code.
        /// </param>
        /// <param name="keyWord">
        /// The key word.
        /// </param>
        /// <returns>
        /// The <see cref="dynamic"/>.
        /// </returns>
        /// <exception cref="AfricasTalkingGatewayException">
        /// </exception>
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

        /// <summary>
        /// The call method.
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <returns>
        /// The <see cref="dynamic"/>.
        /// </returns>
        /// <exception cref="AfricasTalkingGatewayException">
        /// </exception>
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
                var url = this.VoiceUrl + "/call";
                var response = SendPostRequest(data, url);
                dynamic json = JObject.Parse(response);
                return json;
            }
            catch (AfricasTalkingGatewayException e)
            {
                throw new AfricasTalkingGatewayException(e);
            }
        }

        /// <summary>
        /// The get number of queued calls method.
        /// </summary>
        /// <param name="phoneNumber">
        /// The phone number.
        /// </param>
        /// <param name="queueName">
        /// The queue name.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        /// <exception cref="AfricasTalkingGatewayException">
        /// </exception>
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
            var url = this.VoiceUrl + "/queueStatus";
            var response = SendPostRequest(data, url);
            dynamic json = JObject.Parse(response);
            if ((string)json["errorMessage"] == "None")
            {
                return json["entries"];
            }
            throw new AfricasTalkingGatewayException(json["errorMessage"]);
        }

        /// <summary>
        /// The send airtime method.
        /// </summary>
        /// <param name="recepients">
        /// The recepients.
        /// </param>
        /// <returns>
        /// The <see cref="dynamic"/>.
        /// </returns>
        /// <exception cref="AfricasTalkingGatewayException">
        /// </exception>
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

        /// <summary>
        /// The get user data method.
        /// </summary>
        /// <returns>
        /// The <see cref="dynamic"/>.
        /// </returns>
        /// <exception cref="AfricasTalkingGatewayException">
        /// </exception>
        public dynamic GetUserData()
        {
            var urlString = Userdata + "?username=" + _username;
            var response = SendGetRequest(urlString);
            if (_responseCode != (int) HttpStatusCode.OK) throw new AfricasTalkingGatewayException(response);
            dynamic json = JObject.Parse(response);
            return json;
        }

        private string CardOTPValidationURL => PaymentsHost + "/card/checkout/validate";

        private string CardCheckoutURL => PaymentsHost + "/card/checkout/charge";

        private string BankCheckoutURL => PaymentsHost + "/bank/checkout/charge";

        private string OTPValidationURL => PaymentsHost + "/bank/checkout/validate";

        private string BankTransferUrl => PaymentsHost + "/bank/transfer";
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

        /// <summary>
        /// The send get request helper method.
        /// </summary>
        /// <param name="urlString">
        /// The url string.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="AfricasTalkingGatewayException">
        /// </exception>
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
                if (_debug) Console.WriteLine("Full response: " + response);
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

        /// <summary>
        /// The send post request helper method.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="urlString">
        /// The url string.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="AfricasTalkingGatewayException">
        /// </exception>
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
        /// <param name="metadata">
        /// The metadata.
        /// </param>
        /// <returns>
        /// Returns transaction status
        /// </returns>
        /// <exception cref="AfricasTalkingGatewayException">
        /// Throws an error of type 40X 
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
                throw new AfricasTalkingGatewayException(e.StackTrace + e.Message + e.Source);
            }
        }

        //  http://docs.africastalking.com/card/validate

        /// <summary>
        /// Payment Card Checkout Validation APIs allow your application to validate card charge requests that deduct money from a customer's Debit or Credit Card.
        /// </summary>
        /// <param name="transactionId">
        /// This value identifies the transaction that your application wants to validate. This value is contained in the response to the charge request.
        /// </param>
        /// <param name="otp">
        /// This contains the One Time Password that the card issuer sent to the client that owns the payment card.
        /// </param>
        /// <returns>
        /// The <see cref="dynamic"/>.
        /// </returns>
        /// <exception cref="AfricasTalkingGatewayException">
        /// Returns Errors from gateway
        /// </exception>
        public dynamic ValidateCardOtp(string transactionId, string otp)
        {
           var otpValidateCard = new OTP
                                 {
                                     Otp = otp,
                                     TransactionId = transactionId,
                                     Username = this._username
                                 };
            
            try
            {
                var cardOtpResult = this.ValidateOtp(otpValidateCard, this.CardOTPValidationURL);
                return cardOtpResult;
            }
            catch (Exception exception)
            {
                throw new AfricasTalkingGatewayException(exception);
            }
        }

        // http://docs.africastalking.com/card/checkout

        /// <summary>
        /// Payment Card Checkout APIs allow your application to collect money into your Payment Wallet by initiating transactions that deduct money from a customer's Debit or Credit Card.These APIs are currently only available in Nigeria on MasterCard and Verve cards.
        /// </summary>
        /// <param name="productName">
        ///  This value identifies the Africa's Talking Payment Product that should be used to initiate this transaction.
        /// </param>
        /// <param name="paymentCard">
        /// This contains the details of the Payment Card to be charged in this transaction. Please note that you can EITHER provider this or provider a checkoutToken if you have one.
        /// </param>
        /// <param name="currencyCode">
        ///  This is the 3-digit ISO format currency code for the value of this transaction (e.g NGN, USD, KES etc)
        /// </param>
        /// <param name="amount">
        /// This is the amount (in the provided currency) that the mobile subscriber is expected to confirm.
        /// </param>
        /// <param name="narration">
        /// A short description of the transaction that can be displayed on the client's statement.
        /// </param>
        /// <param name="metadata">
        /// his value contains a map of any metadata that you would like us to associate with this request. You can use this field to send data that will map notifications to checkout requests, since we will include it when we send notifications once the checkout is complete.
        /// </param>
        /// <param name="checkoutToken">
        /// This value contains a checkout token that has been generated by our APIs as as result of charging a user's Payment Card in a previous transaction. When using a token, the paymentCard data should NOT be populated
        /// </param>
        /// <returns>
        /// The <see cref="dynamic"/>.
        /// </returns>
        /// <exception cref="AfricasTalkingGatewayException">
        /// Any Errors from our gateway
        /// </exception>
        public dynamic CardCheckout(string productName, PaymentCard paymentCard, string currencyCode, decimal amount, string narration, Dictionary<string, string> metadata = null, string checkoutToken = null)
        {
            var cardCheckout = new CardDetails
            {
                Username = this._username,
                ProductName = productName,
                CurrencyCode = currencyCode,
                PaymentCard = paymentCard,
                Amount = amount,
                Narration = narration
            };
            if (metadata != null)
            {
                cardCheckout.Metadata = metadata;
            }
            if (checkoutToken != null)
            {
                cardCheckout.CheckoutToken = checkoutToken;
            }
            
            try
            {
                var response = this.ProcessCardCheckout(cardCheckout, this.CardCheckoutURL);
                return response;
            }
            catch (Exception exception)
            {
                throw new AfricasTalkingGatewayException(exception);
            }
        }

        // http://docs.africastalking.com/bank/checkout

        /// <summary>
        /// Bank Account checkout APIs allow your application to collect money into your Payment Wallet by initiating an OTP-validated transaction that deducts money from a customer's bank account.
        /// </summary>
        /// <param name="productName">
        /// The product name.
        /// </param>
        /// <param name="bankAccount">
        /// The bank Account.
        /// </param>
        /// <param name="currencyCode">
        /// The currency code.
        /// </param>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <param name="narration">
        /// The narration.
        /// </param>
        /// <param name="metadata">
        /// The metadata.
        /// </param>
        /// <returns>
        /// The <see cref="dynamic"/>.
        /// </returns>
        /// <exception cref="AfricasTalkingGatewayException">
        /// Any Errors thrown by our Gateway
        /// </exception>
        public dynamic BankCheckout(string productName, BankAccount bankAccount, string currencyCode, decimal amount, string narration, Dictionary<string,string> metadata = null)
        {
            var bankCheckout = new BankCheckout()
            {
                Username = this._username,
                ProductName = productName,
                CurrencyCode = currencyCode,
                Amount = amount,
                Narration = narration,
                BankAccount = bankAccount
                                   };
            if (metadata != null)
            {
                bankCheckout.Metadata = metadata;
            }

            try
            {
                var response = this.ProcessBankCheckout(bankCheckout, this.BankCheckoutURL);
                return response;
            }
            catch (Exception exception)
            {
                throw new AfricasTalkingGatewayException(exception);
            }
        }

        /// <summary>
        /// The process bank checkout method.
        /// </summary>
        /// <param name="checkout">
        /// The checkout object.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ProcessBankCheckout(BankCheckout checkout, string url)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("apiKey", this._apikey);
            var result = client.PostAsJsonAsync(url, value: checkout).Result;
            result.EnsureSuccessStatusCode();
            var stringResult = result.Content.ReadAsStringAsync().Result;
            return stringResult;
        }

        // http://docs.africastalking.com/bank/validate

        /// <summary>
        /// Checkout Validation APIs allow your application to validate bank/card charge requests that deduct money from a customer's bank account..
        /// </summary>
        /// <param name="transactionId">
        /// This value identifies the transaction that your application wants to validate. This value is contained in the response to the charge request.
        /// </param>
        /// <param name="otp">
        /// This contains the One Time Password that the bank sent to the client that owns the bank account that is being charged or that the card issuer sent to the client that owns the payment card.
        /// </param>
        /// <returns>
        /// The <see cref="dynamic"/>.
        /// </returns>
        /// <exception cref="AfricasTalkingGatewayException">
        /// Errors from our gateway
        /// </exception>
        public dynamic OtpValidate(string transactionId, string otp)
        {
            var otpValidate = new OTP
            {
                Username = this._username,
                TransactionId = transactionId,
                Otp = otp
            };
            try
            {
                var bankOtpResult = this.ValidateOtp(otpValidate, this.OTPValidationURL);
                return bankOtpResult;
            }
            catch (Exception exception)
            {
                throw new AfricasTalkingGatewayException(exception);
            }
        }

        private string ProcessCardCheckout(CardDetails details, string checkoutURL)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("apiKey", this._apikey);
            var result = client.PostAsJsonAsync(checkoutURL, details).Result;
            result.EnsureSuccessStatusCode();
            var stringResult = result.Content.ReadAsStringAsync().Result;
            return stringResult;
        }

        /// <summary>
        /// You can initiate an OTP Validation request by sending a HTTP POST request.
        /// </summary>
        /// <param name="otp">
        /// The OTP object.
        /// </param>
        /// <param name="url">
        /// The URL to which the POST request is sent.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ValidateOtp(OTP otp, string url)
        {
            var otpClient = new HttpClient();
            otpClient.DefaultRequestHeaders.Add("apiKey", this._apikey);
            var result = otpClient.PostAsJsonAsync(url, otp).Result;
            result.EnsureSuccessStatusCode();
            var otpResult = result.Content.ReadAsStringAsync().Result;
            return otpResult;
        }

        // http://docs.africastalking.com/bank/transfer

        /// <summary>
        /// Initiate Bank Bank Transfer process.
        /// </summary>
        /// <param name="productName">
        /// This value identifies the Africa's Talking Payment Product that should be used to initiate this transaction.
        /// </param>
        /// <param name="recipients">
        /// This contains a list of Recipient elements
        /// </param>
        /// <param name="currencyCode">
        /// The currency code.
        /// </param>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <param name="naration">
        /// A short description of the transaction that can be displayed on the client's statement.
        /// </param>
        /// <param name="metadata">
        /// This value contains a map of any metadata that you would like us to associate with this request. You can use this field to send data that will map notifications to checkout requests, since we will include it when we send notifications once the transaction is complete.
        /// </param>
        /// <returns>
        /// The <see cref="dynamic"/>.
        /// </returns>
        /// <exception cref="AfricasTalkingGatewayException">
        /// </exception>
        public dynamic BankTransfer(string productName, IEnumerable <BankAccountDetails> recipients, string currencyCode, decimal amount, string naration, Dictionary<string, string> metadata = null)
        {
            var transferDetails = new BankTransferDetails
            {
                Recipients = recipients.ToList(),
                CurrencyCode = currencyCode,
                Amount = amount,
                Narration = naration,
                Username = _username
            };
            if (metadata != null)
            {
                transferDetails.Metadata = metadata;
            }

            try
            {
                var bankTransfer = this.ProcessBankTransfer(transferDetails, this.BankTransferUrl);
                return bankTransfer;
            }
            catch (Exception exception)
            {

                throw new AfricasTalkingGatewayException(exception);
            }

        }


        private string ProcessBankTransfer(BankTransfer transfer, string url)
        {
            var transferClient = new HttpClient();
            transferClient.DefaultRequestHeaders.Add("apiKey",this._apikey);
            var transferResult = transferClient.PostAsJsonAsync(this.BankTransferUrl, value: transfer).Result;
            transferResult.EnsureSuccessStatusCode();
            var transferRes = transferResult.Content.ReadAsStringAsync().Result;
            return transferRes;
        }

    /// <summary>
    /// This method handles POST requests to the POST request the B2B endpoint
    /// </summary>
    /// <param name="dataMap">
    /// Structured JSON Object containing all B2B arguments.
    /// </param>
    /// <param name="url">
    /// The B2B End-point.
    /// </param>
    /// <returns>
    /// Server response.
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

        /// <summary>
        /// The mobile b 2 b.
        /// </summary>
        /// <param name="product">
        /// The product.
        /// </param>
        /// <param name="providerChannel">
        /// The provider channel.
        /// </param>
        /// <param name="transfer">
        /// The transfer.
        /// </param>
        /// <param name="currency">
        /// The currency.
        /// </param>
        /// <param name="transferAmount">
        /// The transfer amount.
        /// </param>
        /// <param name="channelReceiving">
        /// The channel receiving.
        /// </param>
        /// <param name="accountReceiving">
        /// The account receiving.
        /// </param>
        /// <param name="btobmetadata">
        /// Metadata Associated With the transaction.
        /// </param>
        /// <returns>
        /// The <see cref="dynamic"/>.
        /// </returns>
        /// <exception cref="AfricasTalkingGatewayException">
        /// Any errors thrown by our Gateway
        /// </exception>
        public dynamic MobileB2B(
            string product,
            string providerChannel,
            string transfer,
            string currency,
            decimal transferAmount,
            string channelReceiving,
            string accountReceiving,
            dynamic btobmetadata)
        {
            var btob = new B2BData
            {
                username = _username,
                productName = product,
                provider = providerChannel,
                transferType = transfer,
                currencyCode = currency,
                amount = transferAmount,
                destinationAccount = accountReceiving,
                destinationChannel = channelReceiving,
                metadata = btobmetadata
            };
            try
            {
                var response = PostB2BJson(btob, B2BPaymentsUrl);
                return response;
            }
            catch (Exception e)
            {
                throw new AfricasTalkingGatewayException(e);
            }
        }

        /// <summary>
        /// The mobile Business to Client method.
        /// </summary>
        /// <param name="productName">
        /// The product name.
        /// </param>
        /// <param name="recepients">
        /// The recepients.
        /// </param>
        /// <returns>
        /// The <see cref="dynamic"/>.
        /// </returns>
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

        /// <summary>
        /// The post helper method.
        /// </summary>
        /// <param name="requestBody">
        /// The request body.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="DataResult"/>.
        /// </returns>
        private DataResult Post(RequestBody requestBody, string url)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("apikey", _apikey);
             var res = httpClient.PostAsJsonAsync(url, requestBody).Result;
            res.EnsureSuccessStatusCode();
            var result = res.Content.ReadAsAsync<DataResult>();
            return result.Result;
        }

        /// <summary>
        /// The post as json helper method.
        /// </summary>
        /// <param name="dataMap">
        /// The data map.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string PostAsJson(CheckOutData dataMap, string url)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("apiKey", _apikey);
            var result = client.PostAsJsonAsync(url, dataMap).Result;
            result.EnsureSuccessStatusCode();
            var stringResult = result.Content.ReadAsStringAsync().Result;
            return stringResult;
        }
        
        /// <summary>
        /// The remote certificate validation callback for SSL validations.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="certificate">
        /// The certificate.
        /// </param>
        /// <param name="chain">
        /// The chain.
        /// </param>
        /// <param name="sslpolicyerrors">
        /// The sslpolicyerrors.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool RemoteCertificateValidationCallback(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslpolicyerrors)
        {
            return true;
        }
    }
}
