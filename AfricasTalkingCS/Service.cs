using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace AfricasTalkingSDK
{
    abstract class Service
    {
        const string BASE_DOMAIN        = "africastalking.com";
        const string BASE_SANDBOX_DOMAIN = "sandbox." + BASE_DOMAIN;

        const string INTL_PHONE_FORMAT = "^\\+\\d{1,3}\\d{3,}$";
        string _username;
        string _apiKey;
        string _baseUrl;
        string _serviceURL;

        public Service(string username, string apiKey, string serviceName)
        {
            _username = username;
            _apiKey = apiKey;
            SetBaseUrl();
            SetServiceURL(serviceName);
        }

        public Service(string username, string apiKey, string baseUrlPrefix, string serviceName)
        {
            _username = username;
            _apiKey = apiKey;
            SetBaseUrl(baseUrlPrefix);
            SetServiceURL(serviceName);
        }
        public Service() { }

        protected abstract dynamic GetInstance(string username, string apiKey);

        private void SetBaseUrl()
        {
            if (_username.ToLower() == "sandbox")
            {
                _baseUrl = "https://api." + BASE_SANDBOX_DOMAIN + "/version1";
            }
            else
            {
                _baseUrl = "https://api." + BASE_DOMAIN + "/version1";
            }
        }

        private void SetBaseUrl(string baseUrlPrefix)
        {
            if (_username.ToLower() == "sandbox")
            {
                _baseUrl = "https://api." + baseUrlPrefix  + BASE_SANDBOX_DOMAIN + "/version1";
            }
            else
            {
                _baseUrl = "https://api." + baseUrlPrefix + BASE_DOMAIN + "/version1";
            }
        }

        protected void SetServiceURL(string ServiceName)
        {
            if(_baseUrl == null)
            {
                SetBaseUrl();
            }
            _serviceURL = _baseUrl + "/" + ServiceName.ToLower();
        }

        public string MakeRequest(string urlSegment, string method, IDictionary data)
        {
            data.Add("username", this._username);
            //
            if (_serviceURL != null)
            {
                if (method.ToLower() == "get")
                {
                    string requestUrl = _serviceURL + "/" + urlSegment;
                    return MakeGetRequest(data, requestUrl);
                }
                else if (method.ToLower() == "post")
                {
                    string requestUrl = _serviceURL + "/" + urlSegment;
                    return MakePostRequest(data, requestUrl);
                }

                return "Invalid Method"; // Exception here
            } else
            {
                // Throw exception here
                return "";
            }

        }

        public string MakeRequest(string urlSegment, IDictionary data)
        {
            //
            if (_serviceURL != null)
            {
                string requestUrl = _serviceURL + "/" + urlSegment;
                return MakeGetRequest(data, requestUrl);
            }
            else
            {
                // Throw exception here
                return "";
            }

        }

        public string MakeRequest(string urlSegment)
        {
            //
            if (_serviceURL != null)
            {
                string requestUrl = _serviceURL + "/" + urlSegment;
                return MakeGetRequest(requestUrl);
            }
            else
            {
                // Throw exception here
                return "";
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
        private string MakePostRequest(IDictionary data, string urlString)
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
                var webRequest = (HttpWebRequest) WebRequest.Create(urlString);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = byteArray.Length;
                webRequest.Accept = "application/json";
                webRequest.Headers.Add("apiKey", this._apiKey);

                var webpageStream = webRequest.GetRequestStream();
                webpageStream.Write(byteArray, 0, byteArray.Length);
                webpageStream.Close();

                var httpResponse = (HttpWebResponse) webRequest.GetResponse();
                var webpageReader = new StreamReader(
                    httpResponse.GetResponseStream() ?? throw new InvalidOperationException());
                var response = webpageReader.ReadToEnd();
                return response;
            }
            catch (WebException webException)
            {
                if (webException.Response == null)
                {
//                    throw new AfricasTalkingGatewayException(webException.Message);
                }

                using (var stream = webException.Response.GetResponseStream())
                using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
                {
                    var response = reader.ReadToEnd();
                    return response;
                }
            }
        }

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
        private string MakeGetRequest(IDictionary data, string urlString)
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

                var requestUrl = urlString + "?" + dataStr;
         
                ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;
                var webRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
                webRequest.Method = "GET";
                webRequest.Accept = "application/json";
                webRequest.Headers.Add("apiKey", this._apiKey);
                var httpResponse = (HttpWebResponse)webRequest.GetResponse();
                var webpageReader = new StreamReader(httpResponse.GetResponseStream() ?? throw new InvalidOperationException());
                var response = webpageReader.ReadToEnd();
                return response;
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                {
//                    throw new AfricasTalkingGatewayException(ex.Message);
                }

                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
                {
                    var response = reader.ReadToEnd();
                    return response;
                }
            }
        }

        private string MakeGetRequest(string urlString)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;
                var webRequest = (HttpWebRequest)WebRequest.Create(urlString);
                webRequest.Method = "GET";
                webRequest.Accept = "application/json";
                webRequest.Headers.Add("apiKey", this._apiKey);
                var httpResponse = (HttpWebResponse)webRequest.GetResponse();
                var webpageReader = new StreamReader(httpResponse.GetResponseStream() ?? throw new InvalidOperationException());
                var response = webpageReader.ReadToEnd();
                return response;
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                {
                    //                    throw new AfricasTalkingGatewayException(ex.Message);
                }

                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
                {
                    var response = reader.ReadToEnd();
                    return response;
                }
            }

        }

        protected bool CheckPhoneNumber(string phone)
        {
            Regex r = new Regex(INTL_PHONE_FORMAT, RegexOptions.IgnoreCase);
            Match m = r.Match(phone);
            if (!m.Success) 
            {
                throw new Exception("Invalid phone number: " + phone + "; Expecting number in format +XXXxxxxxxxxx");
            }
            return true;
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
