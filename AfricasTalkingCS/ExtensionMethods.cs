using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace AfricasTalkingCS
{
    public static class ExtensionMethods
    {
        public static bool IsValid(this string s) {
            if (!string.IsNullOrWhiteSpace(s)) {
                return s.Length > 0;
            }
            return false;
        }

        public static int ToInt(this HttpStatusCode code) => (int)code;

        public static string ToJson<T>(this T model)
        {
            if (model == null)
                return string.Empty;
            return JsonConvert.SerializeObject(model);
        }

        public static TResult FromJsonTo<TResult>(this string json)
            => JsonConvert.DeserializeObject<TResult>(json);

        public static IEnumerable<KeyValuePair<string,string>> CastToCollection(this IDictionary dictionary)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            if (dictionary == null)
                return list;
            IDictionaryEnumerator enumerator = dictionary.GetEnumerator();

            while (enumerator.MoveNext()) {
                var item = (DictionaryEntry)enumerator.Current;
                list.Add(new KeyValuePair<string, string>(item.Key.ToString(), item.Value.ToString()));
            }
            return list;
        }
    }
}