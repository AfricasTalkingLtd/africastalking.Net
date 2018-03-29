namespace AfricasTalkingSDK
{
    using System;

    public static class AfricasTalkingSDK
    {
        public static string SMS_SERVICE = "SMS.SmsService";
        
        private static string _username;
        private static string _apiKey;

        public static void Initialize(string username, string apiKey)
        {
            _username = username;
            _apiKey = apiKey;
        }

        public static dynamic GetService(string serviceName)
        {
            try
            {
                var myType = typeof(AfricasTalkingSDK);
                var t = GetType(myType.Namespace + "." + serviceName);
                var instance = Activator.CreateInstance(t);
                return instance;
            }
            catch
            {
                return null;
            }
        }

        private static Type GetType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null) return type;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }
    }
}
