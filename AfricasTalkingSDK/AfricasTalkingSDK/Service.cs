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
    public abstract class Service
    {
        const string BaseDomain        = "africastalking.com";
        const string BaseSandboxDomain = "sandbox." + BaseDomain;

        const string IntlPhoneFormat = "^\\+\\d{1,3}\\d{3,}$";
        readonly string _username;
        readonly string _apiKey;
        string _baseUrl;
        string _serviceUrl;

        public Service(string username, string apiKey, string serviceName)
        {
            _username = username;
            _apiKey = apiKey;
            SetBaseUrl();
            SetServiceUrl(serviceName);
        }

        public Service(string username, string apiKey, string baseUrlPrefix, string serviceName)
        {
            _username = username;
            _apiKey = apiKey;
            SetBaseUrl(baseUrlPrefix);
            SetServiceUrl(serviceName);
        }

        public Service() { }

        protected abstract dynamic GetInstance(string username, string apiKey);

        private void SetBaseUrl()
        {
            if (_username.ToLower() == "sandbox")
            {
                _baseUrl = "https://api." + BaseSandboxDomain + "/version1";
            }
            else
            {
                _baseUrl = "https://api." + BaseDomain + "/version1";
            }
        }

        private void SetBaseUrl(string baseUrlPrefix)
        {
            if (_username.ToLower() == "sandbox")
            {
                _baseUrl = "https://api." + baseUrlPrefix  + BaseSandboxDomain + "/version1";
            }
            else
            {
                _baseUrl = "https://api." + baseUrlPrefix + BaseDomain + "/version1";
            }
        }

        private void SetServiceUrl(string serviceName)
        {
            if(_baseUrl == null)
            {
                SetBaseUrl();
            }
            _serviceUrl = _baseUrl + "/" + serviceName.ToLower();
        }

        public string MakeRequest(string urlSegment, string method, IDictionary data)
        {
            data.Add("username", _username);
            if (_serviceUrl != null)
            {
                if (method.ToLower() == "get")
                {
                    string requestUrl = _serviceUrl + "/" + urlSegment;
                    return MakeGetRequest(data, requestUrl);
                }
                else if (method.ToLower() == "post")
                {
                    string requestUrl = _serviceUrl + "/" + urlSegment;
                    return MakePostRequest(data, requestUrl);
                }

                return "Invalid Method"; 
            }

            return "";

        }

        public string MakeRequest(string urlSegment, IDictionary data)
        {
            if (_serviceUrl != null)
            {
                string requestUrl = _serviceUrl + "/" + urlSegment;
                return MakeGetRequest(data, requestUrl);
            }

            return "";

        }

        public string MakeRequest(string urlSegment)
        {
            if (_serviceUrl == null) return "";
            var requestUrl = _serviceUrl + "/" + urlSegment;
            return MakeGetRequest(requestUrl);
        }

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
                webRequest.Headers.Add("apiKey", _apiKey);

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
                    throw new AfricasTalkingGatewayException(webException.Message);
                }

                using (var stream = webException.Response?.GetResponseStream())
                using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
                {
                    var response = reader.ReadToEnd();
                    return response;
                }
            }
        }

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
                webRequest.Headers.Add("apiKey", _apiKey);
                var httpResponse = (HttpWebResponse)webRequest.GetResponse();
                var webpageReader = new StreamReader(httpResponse.GetResponseStream() ?? throw new InvalidOperationException());
                var response = webpageReader.ReadToEnd();
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
                webRequest.Headers.Add("apiKey", _apiKey);
                var httpResponse = (HttpWebResponse)webRequest.GetResponse();
                var webpageReader = new StreamReader(httpResponse.GetResponseStream() ?? throw new InvalidOperationException());
                var response = webpageReader.ReadToEnd();
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
                    return response;
                }
            }

        }

        protected bool CheckPhoneNumber(string phone)
        {
            var r = new Regex(IntlPhoneFormat, RegexOptions.IgnoreCase);
            var m = r.Match(phone);
            if (!m.Success) 
            {
                throw new Exception("Invalid phone number: " + phone + "; Expecting number in format +XXXxxxxxxxxx");
            }
            return true;
        }


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
