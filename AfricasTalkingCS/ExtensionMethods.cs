using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace AfricasTalkingCS
{
    public static class ExtensionMethods
    {
        public static string ToJson<T>(this T model)
        {
            if (model == null)
                return string.Empty;
            return JsonConvert.SerializeObject(model);
        }
        public static TResult FromJsonTo<TResult>(this string json)
            => JsonConvert.DeserializeObject<TResult>(json);

        public static int ToInt(this HttpStatusCode code) => (int)code;
    }
}