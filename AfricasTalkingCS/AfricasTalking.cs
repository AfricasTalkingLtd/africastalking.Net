using System;

namespace AfricasTalkingSDK
{
    public class AfricasTalking
    {

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
                var myType = typeof(AfricasTalking);
                Type t = GetType(myType.Namespace + "." + serviceName);
                object instance = Activator.CreateInstance(t);//, new object[] { _username, _apiKey });
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
