﻿// --------------------------------------------------------------------------------------------------------------------
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
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Text.RegularExpressions;
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
        /// The username. Expects "sandbox" or Actual AfricasTalking username
        /// </param>
        /// <param name="apikey">
        /// The apikey.
        /// </param>
        public AfricasTalkingGateway(string username,string apikey)
        {
            if (username == "sandbox")
            {
                _username = "sandbox";
                _environment = "sandbox";
                _apikey = apikey;
            }
            else
            {
                _username = username;
                _apikey = apikey;
                _environment = "production";
            }
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
        /// The Recipient(s).
        /// </param>
        /// <param name="message">
        /// The message content.
        /// </param>
        /// <param name="from">
        /// The Sender.
        /// </param>
        /// <param name="bulkSmsMode">
        /// The bulk SMS mode: set 1 for Bulk SMS
        /// </param>
        /// <param name="options">
        /// The Options for premium SMS.
        /// </param>
        /// <returns>
        /// The <see cref="dynamic"/>.
        /// </returns>
        /// <exception cref="AfricasTalkingGatewayException">
        /// Errors thrown by the gateway
        /// </exception>
        public dynamic SendMessage(
            string to,
            string message,
            string from = null,
            int bulkSmsMode = -1,
            Hashtable options = null)
        {
            // TODO Convert options to type IDictionary
            var isValidphoneNumber = IsPhoneNumber(to);
            if (to.Length == 0 || message.Length == 0 || !isValidphoneNumber)
            {
                throw new AfricasTalkingGatewayException("The message is either empty or phone number is not valid");
            }
            else
            {
                try
                {
                    var data = new Hashtable
                                   {
                                       ["username"] = this._username,
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
                            {
                                data["retryDurationInHours"] = options["retryDurationInHours"];
                            }
                        }
                    }

                    var response = this.SendPostRequest(data, this.SmsUrl);
                    dynamic json = JObject.Parse(response);
                    return json;
                }
                catch (AfricasTalkingGatewayException e)
                {
                    throw new AfricasTalkingGatewayException(e);
                }
            }
        }

        /// <summary>
        /// Checks if the number given is a valid phoneNumber.
        /// </summary>
        /// <param name="number">
        /// The phone number.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^\+?(\d[\d-. +,]+)?(\([\d-. +,]+\))?[\d-. ]+\d$").Success && number.Length > 5;
        }

        /// <summary>
        /// The upload media file method.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <exception cref="AfricasTalkingGatewayException">
        /// Errors from our gateway class
        /// </exception>
        public void UploadMediaFile(string url)
        {
            var isUrl = Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute);
            if (!isUrl)
            {
              throw new AfricasTalkingGatewayException("Malformed Url");
            }
            else
            {
                var data = new Hashtable
                               {
                                   ["username"] = this._username,
                                   ["url"] = url
                               };
                var urlString = this.VoiceUrl + "/mediaUpload";
                var response = this.SendPostRequest(data, urlString);
                dynamic json = JObject.Parse(response);
                if ((string)json["errorMesage"] != "None")
                {
                    throw new AfricasTalkingGatewayException(json["errorMessage"]);
                }
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
        /// Errors thrown by our gateway
        /// </exception>
        public dynamic FetchMessages(int lastReceivedId)
        {
            var url = this.SmsUrl + "?username=" + this._username + "&lastReceivedId" + Convert.ToString(lastReceivedId);
            var response = this.SendGetRequest(url);

            if (this._responseCode != (int)HttpStatusCode.OK)
            {
                throw new AfricasTalkingGatewayException(response);
            }

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
        /// Any error thrown by our gateway class
        /// </exception>
        public dynamic CreateSubscription(string phoneNumber, string shortCode, string keyWord, string checkoutToken)
        {
            if (phoneNumber.Length == 0 || shortCode.Length == 0 || keyWord.Length == 0 || checkoutToken.Length == 0 || !IsPhoneNumber(phoneNumber))
            {
                throw new AfricasTalkingGatewayException("Some Parameters are missing or not properly formatted");
            }

            var data = new Hashtable
            {
                ["username"] = this._username,
                ["phoneNumber"] = phoneNumber,
                ["shortCode"] = shortCode,
                ["keyword"] = keyWord,
                ["checkoutToken"] = checkoutToken
            };
            var url = this.SubscriptionUrl + "/create";
            var response = this.SendPostRequest(data, url);
            if (this._responseCode != (int)HttpStatusCode.Created)
            {
                throw new AfricasTalkingGatewayException(response);
            }

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
        /// Errors thrown by our gateway
        /// </exception>
        public dynamic DeleteSubscription(string phoneNumber, string shortCode, string keyWord)
        {
            if (phoneNumber.Length == 0 || shortCode.Length == 0 || keyWord.Length == 0 || !IsPhoneNumber(phoneNumber))
            {
                throw new AfricasTalkingGatewayException("Some Parameters are missing or phonenumber is malformed");
            }
            else
            {
                var data = new Hashtable
                               {
                                   ["username"] = this._username,
                                   ["phoneNumber"] = phoneNumber,
                                   ["shortCode"] = shortCode,
                                   ["keyword"] = keyWord
                               };
                var url = this.SubscriptionUrl + "/delete";
                var response = this.SendPostRequest(data, url);
                if (this._responseCode != (int)HttpStatusCode.Created)
                {
                    throw new AfricasTalkingGatewayException(response);
                }

                dynamic json = JObject.Parse(response);
                return json;
            }
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
        /// Errors from our gateway
        /// </exception>
        public dynamic Call(string from, string to)
        {
            var numbersAreValid = IsPhoneNumber(from) && IsPhoneNumber(to);
            if (!numbersAreValid)
            {
                throw new AfricasTalkingGatewayException("One or both of the phonenumber(s) provided is (are) not valid");
            }
            else
            {
                var data = new Hashtable
                {
                    ["username"] = this._username,
                    ["from"] = from,
                    ["to"] = to
                };
                //var data = new Dictionary<string, string>
                // {
                //    { "username", this._username },
                //    { "from", from },
                //    { "to", to }
                // };
                try
                {
                    var url = this.VoiceUrl + "/call";
                    var response = this.SendPostRequest(data, url);
                    // dynamic json = JObject.Parse(response);
                    return response;
                }
                catch (AfricasTalkingGatewayException e)
                {
                    throw new AfricasTalkingGatewayException(e);
                }
            }
        }

       // private  static string RawConvert()

        /// <summary>
        /// Allows one the developer to create a checkout token to be used in a subscription or USSD push.
        /// </summary>
        /// <param name="phoneNumber">
        /// A valid phone number.
        /// </param>
        /// <returns>
        /// The <see cref="dynamic"/>.
        /// </returns>
        public dynamic CreateCheckoutToken(string phoneNumber)
        {
            if (!IsPhoneNumber(phoneNumber))
            {
                throw new AfricasTalkingGatewayException("The phone number supplied is not valid");
            }
            else
            {
                try
                {
                    var payload = new Hashtable
                                      {
                                          ["phoneNumber"] = phoneNumber
                                      };
                    var response = this.SendPostRequest(payload, this.TokenUrl);
                    dynamic tokenRes = JObject.Parse(response);
                    return tokenRes;
                }
                catch (AfricasTalkingGatewayException e)
                {
                    throw new AfricasTalkingGatewayException("An error ocurred while creating this token: " + e.Message);
                }
            }
        }

        /// <summary>
        /// Initiates USSD push request.
        /// </summary>
        /// <param name="phoneNumber">
        /// The phone number.
        /// </param>
        /// <param name="prompt">
        /// The prompt.This is the USSD menu to be displayed. Must start with CON or END
        /// </param>
        /// <param name="checkoutToken">
        /// The checkout token.
        /// </param>
        /// <returns>
        /// The <see cref="dynamic"/>.
        /// </returns>
        /// <exception cref="AfricasTalkingGatewayException">
        /// Errors from the gateway
        /// </exception>
        public dynamic InitiateUssdPushRequest(string phoneNumber, string prompt, string checkoutToken)
        {
            if (!IsValidToken(checkoutToken) || prompt.Length == 0 || !IsPhoneNumber(phoneNumber))
            {
                throw new AfricasTalkingGatewayException("One or some of the arguments supplied are invalid.");
            }

            try
            {
                var data = new Hashtable
                               {
                                   ["username"] = this._username,
                                   ["phoneNumber"] = phoneNumber,
                                   ["menu"] = prompt,
                                   ["checkoutToken"] = checkoutToken
                               };
                var apiPath = this.UssdPushUrl;
                var response = this.SendPostRequest(data, apiPath);
                dynamic res = JObject.Parse(response);
                return res;
            }
            catch (Exception e)
            {
               throw new AfricasTalkingGatewayException(e.Message + e.StackTrace);
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
        /// Errors from our gateway class
        /// </exception>
        public int GetNumberOfQueuedCalls(string phoneNumber, string queueName = null)
        {
            if (!IsPhoneNumber(phoneNumber))
            {
                throw new AfricasTalkingGatewayException("Phone Number is invalid");
            }

            {
                var data = new Hashtable
                               {
                                   ["username"] = this._username,
                                   ["phoneNumbers"] = phoneNumber,
                               };
                if (queueName != null)
                {
                    data["queueName"] = queueName;
                }

                var url = this.VoiceUrl + "/queueStatus";
                var response = this.SendPostRequest(data, url);
                dynamic json = JObject.Parse(response);
                if ((string)json["errorMessage"] == "None")
                {
                    return json["entries"];
                }

                throw new AfricasTalkingGatewayException(json["errorMessage"]);
            }
        }

        /// <summary>
        /// The send airtime method.
        /// </summary>
        /// <param name="recipients">
        /// The recipients.
        /// </param>
        /// <returns>
        /// The <see cref="dynamic"/>.
        /// </returns>
        /// <exception cref="AfricasTalkingGatewayException">
        /// Errors thrown by our gateway
        /// </exception>
        public dynamic SendAirtime(dynamic recipients)
        {
            if (recipients == null)
            {
                throw new AfricasTalkingGatewayException("Your recepients list is malformed");
            }

            var urlString = this.AirtimeUrl + "/send";
            var recipientsList = JObject.Parse(recipients);
            var data = new Hashtable { ["username"] = this._username, ["recipients"] = "[" + recipientsList + "]" };
            try
            {
                var response = this.SendPostRequest(data, urlString);
                if (this._responseCode != (int)HttpStatusCode.Created)
                {
                    throw new AfricasTalkingGatewayException(response);
                }

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
        /// Errors thrown by our gateway
        /// </exception>
        public dynamic GetUserData()
        {
            var urlString = this.Userdata + "?username=" + this._username;
            var response = this.SendGetRequest(urlString);
            if (this._responseCode != (int)HttpStatusCode.OK)
            {
                throw new AfricasTalkingGatewayException(response);
            }

            dynamic json = JObject.Parse(response);
            return json;
        }

        /// <summary>
        /// The card OTP validation url.
        /// </summary>
        private string CardOtpValidationUrl => this.PaymentsHost + "/card/checkout/validate";

        /// <summary>
        /// The card checkout url.
        /// </summary>
        private string CardCheckoutUrl => this.PaymentsHost + "/card/checkout/charge";

        /// <summary>
        /// The bank checkout url.
        /// </summary>
        private string BankCheckoutUrl => this.PaymentsHost + "/bank/checkout/charge";

        /// <summary>
        /// The OTP validation url.
        /// </summary>
        private string OtpValidationUrl => this.PaymentsHost + "/bank/checkout/validate";

        /// <summary>
        /// The bank transfer url.
        /// </summary>
        private string BankTransferUrl => this.PaymentsHost + "/bank/transfer";

        /// <summary>
        /// Main payments endpoint.
        /// </summary>
        private string PaymentsUrl => this.PaymentsHost + "/mobile/checkout/request";

        /// <summary>
        /// Business to Business API endpoint.
        /// </summary>
        private string B2BPaymentsUrl => this.PaymentsHost + "/mobile/b2b/request";

        /// <summary>
        /// Business to Client Endpoint.
        /// </summary>
        private string B2CPaymentsUrl => this.PaymentsHost + "/mobile/b2c/request";

        /// <summary>
        /// Subscription endpoint.
        /// </summary>
        private string SubscriptionUrl => this.ApiHost + "/version1/subscription";

        /// <summary>
        /// The user data endpoint.
        /// </summary>
        private string Userdata => this.ApiHost + "/version1/user";

        /// <summary>
        /// Airtime Endpoint.
        /// </summary>
        private string AirtimeUrl => this.ApiHost + "/version1/airtime";

        /// <summary>
        /// Voice endpoint.
        /// </summary>
        private string VoiceUrl => (ReferenceEquals(this._environment, "sandbox")
            ? "https://voice.sandbox.africastalking.com"
            : "https://voice.africastalking.com");

        /// <summary>
        /// SMS Endpoint.
        /// </summary>
        private string SmsUrl => this.ApiHost + "/version1/messaging";

        /// <summary>
        /// Root API host.
        /// </summary>
        private string ApiHost => (ReferenceEquals(this._environment, "sandbox")
                                       ? "https://api.sandbox.africastalking.com"
                                       : "https://api.africastalking.com");

        /// <summary>
        /// Payment endpoint.
        /// </summary>
        private string PaymentsHost => (ReferenceEquals(this._environment, "sandbox") ? "https://payments.sandbox.africastalking.com" : "https://payments.africastalking.com");

        /// <summary>
        /// The token creation  url.
        /// </summary>
        private string TokenUrl => this.ApiHost + "/checkout/token/create";

        /// <summary>
        /// The USSD push url.
        /// </summary>
        private string UssdPushUrl => this.ApiHost + "/ussd/push/request";

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
        /// Errors from our gateway class.
        /// </exception>
        private string SendGetRequest(string urlString)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;
                var webRequest = (HttpWebRequest)WebRequest.Create(urlString);
                webRequest.Method = "GET";
                webRequest.Accept = "application/json";
                webRequest.Headers.Add("apiKey", this._apikey);
                var httpResponse = (HttpWebResponse)webRequest.GetResponse();
                this._responseCode = (int)httpResponse.StatusCode;
                var webpageReader = new StreamReader(httpResponse.GetResponseStream() ?? throw new InvalidOperationException());
                var response = webpageReader.ReadToEnd();
                if (this._debug)
                {
                    Console.WriteLine("Full response: " + response);
                }

                return response;
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                {
                    throw new AfricasTalkingGatewayException(ex.Message);
                }

                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
                {
                    var response = reader.ReadToEnd();

                    if (this._debug)
                    {
                        Console.WriteLine("Full response: " + response);
                    }

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
        /// Errors from our gateway class.
        /// </exception>
        private string SendPostRequest(IDictionary data, string urlString)
        {
            try
            {
                var dataStr = string.Empty;
                foreach (string key in data.Keys)
                {
                    if (dataStr.Length > 0)
                    {
                        dataStr += "&";
                    }
                    
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
                webRequest.Headers.Add("apiKey", this._apikey);

                var webpageStream = webRequest.GetRequestStream();
                webpageStream.Write(byteArray, 0, byteArray.Length);
                webpageStream.Close();

                var httpResponse = (HttpWebResponse)webRequest.GetResponse();
                this._responseCode = (int)httpResponse.StatusCode;
                var webpageReader = new StreamReader(
                    httpResponse.GetResponseStream() ?? throw new InvalidOperationException());
                var response = webpageReader.ReadToEnd();

                if (this._debug)
                {
                    Console.WriteLine("Response: " + response);
                }

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
                    if (this._debug)
                    {
                        Console.WriteLine("Exception: " + response);
                    }

                    return response;
                }
            }
        }

        /// <summary>
        /// Checks if the users supplied a valid currency symbol.
        /// </summary>
        /// <param name="isoCurrency">
        /// The ISO currency.
        /// </param>
        /// <param name="symbol">
        /// The symbol.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsValidCurrency(string isoCurrency, out string symbol)
        {
            symbol = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(c => !c.IsNeutralCulture).Select(
                    culture =>
                        {
                            try
                            {
                                return new RegionInfo(culture.LCID);
                            }
                            catch 
                            {
                                return null;
                            }
                        }).Where(ri => ri != null && ri.ISOCurrencySymbol == isoCurrency)
                .Select(ri => ri.CurrencySymbol)
                .FirstOrDefault();
            return symbol != null;
        }

        /// <summary>
        /// Checks if the correct transaction ID is supplied.
        /// </summary>
        /// <param name="transactionId">
        /// The transaction ID.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsValidTransactionId(string transactionId)
        {
            return Regex.Match(transactionId, @"^ATPid_.*$").Success && transactionId.Length > 7;
        }

        /// <summary>
        /// Checks if the token supplied is valid.
        /// </summary>
        /// <param name="token">
        /// The token issued by the API from the CreateCheckoutToken method.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsValidToken(string token)
        {
            return Regex.Match(token, @"^CkTkn_.*$").Success && token.Length > 7;
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
        public dynamic Checkout(string productName, string phoneNumber, string currencyCode, decimal amount, string providerChannel, Dictionary<string, string> metadata = null)
        {
            string symbol;
            
            if (productName.Length == 0 || !IsPhoneNumber(phoneNumber) || !IsValidCurrency(currencyCode, out symbol) || providerChannel.Length == 0)
            {
                throw new AfricasTalkingGatewayException("Missing or malformed arguments, or invalid currency symbol or phonenumber");
            }

            var checkout = new CheckOutData
            {
                username = this._username,
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
                var checkoutResponse = this.PostAsJson(checkout, this.PaymentsUrl);
                return checkoutResponse;
            }
            catch (Exception e)
            {
                throw new AfricasTalkingGatewayException(e.StackTrace + e.Message + e.Source);
            }
        }

        // http://docs.africastalking.com/card/validate

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
            if (transactionId.Length < 32 || otp.Length < 3 || !IsValidTransactionId(transactionId))
            {
                throw new AfricasTalkingGatewayException("Incorrect Transaction ID or invalid OTP length");
            }

           var otpValidateCard = new OTP
                                 {
                                     Otp = otp,
                                     TransactionId = transactionId,
                                     Username = this._username
                                 };
            
            try
            {
                var cardOtpResult = this.ValidateOtp(otpValidateCard, this.CardOtpValidationUrl);
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
        /// Any errors from our gateway
        /// </exception>
        public dynamic CardCheckout(string productName, PaymentCard paymentCard, string currencyCode, decimal amount, string narration, Dictionary<string, string> metadata = null, string checkoutToken = null)
        {
            string curSym;
            if (productName.Length != 0 && IsValidCurrency(currencyCode, out curSym) && narration.Length != 0)
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
                    var response = this.ProcessCardCheckout(cardCheckout, this.CardCheckoutUrl);
                    return response;
                }
                catch (Exception exception)
                {
                    throw new AfricasTalkingGatewayException(exception);
                }
            }
            else
            {
                throw new AfricasTalkingGatewayException("Invalid arguments");
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
        public dynamic BankCheckout(
            string productName,
            BankAccount bankAccount,
            string currencyCode,
            decimal amount,
            string narration,
            Dictionary<string, string> metadata = null)
        {
            string curSym;
            if (productName.Length != 0 && IsValidCurrency(currencyCode, out curSym) && narration.Length != 0)
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
                    var response = this.ProcessBankCheckout(bankCheckout, this.BankCheckoutUrl);
                    return response;
                }
                catch (Exception exception)
                {
                    throw new AfricasTalkingGatewayException(exception);
                }
            }
            else
            {
                    throw new AfricasTalkingGatewayException("Invalid arguments");
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
                var bankOtpResult = this.ValidateOtp(otpValidate, this.OtpValidationUrl);
                return bankOtpResult;
            }
            catch (Exception exception)
            {
                throw new AfricasTalkingGatewayException(exception);
            }
        }

        /// <summary>
        /// The process card checkout.
        /// </summary>
        /// <param name="details">
        /// The details.
        /// </param>
        /// <param name="checkoutUrl">
        /// The checkout url.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ProcessCardCheckout(CardDetails details, string checkoutUrl)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("apiKey", this._apikey);
            var result = client.PostAsJsonAsync(checkoutUrl, details).Result;
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
        /// <returns>
        /// The <see cref="dynamic"/>.
        /// </returns>
        /// <exception cref="AfricasTalkingGatewayException">
        /// Errors from our gateway class
        /// </exception>
        public dynamic BankTransfer(string productName, IEnumerable<BankTransferRecipients> recipients)
        {
            if (productName.Length == 0)
            {
                throw new AfricasTalkingGatewayException("Not a valid product name");
            }

            var transferDetails = new BankTransfer()
            {
                Recipients = recipients.ToList(),
                ProductName = productName,
                Username = this._username
            };

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

        /// <summary>
        /// The process bank transfer.
        /// </summary>
        /// <param name="transfer">
        /// The transfer.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ProcessBankTransfer(BankTransfer transfer, string url)
        {
            var transferClient = new HttpClient();
            transferClient.DefaultRequestHeaders.Add("apiKey", this._apikey);
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
        /// The mobile B2B.
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
            string cSym;
            if (product.Length == 0 || providerChannel.Length == 0 || transfer.Length == 0
                || !IsValidCurrency(currency, out cSym) || channelReceiving.Length == 0 || accountReceiving.Length == 0)
            {
                throw new AfricasTalkingGatewayException("Invalid arguments");
            }

            var btob = new B2BData
            {
                username = this._username,
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
                var response = this.PostB2BJson(btob, this.B2BPaymentsUrl);
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
        /// <param name="recipients">
        /// The recipients.
        /// </param>
        /// <returns>
        /// The <see cref="dynamic"/>.
        /// </returns>
        public dynamic MobileB2C(string productName, IEnumerable<MobileB2CRecepient> recipients)
        {
            if (productName.Length == 0)
            {
                throw new AfricasTalkingGatewayException("Malformed product name");
            }

            var requestBody = new RequestBody
            {
                productName = productName,
                username = this._username,
                recepients = recipients.ToList()
            };
            var response = this.Post(requestBody, this.B2CPaymentsUrl);
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
            httpClient.DefaultRequestHeaders.Add("apikey", this._apikey);
             var res = httpClient.PostAsJsonAsync(url, requestBody).Result;
            res.EnsureSuccessStatusCode();
            var result = res.Content.ReadAsAsync<DataResult>();
            return result.Result;
        }

        /// <summary>
        /// The post as JSON helper method.
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

            client.DefaultRequestHeaders.Add("apiKey", this._apikey);
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
        /// The SSL policy errors.
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
