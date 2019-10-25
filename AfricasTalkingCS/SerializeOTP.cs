using Newtonsoft.Json;

namespace AfricasTalkingCS
{
    public static class SerializeOTP
    {
        public static string OTPToJson(this OTP self)
        {
            return JsonConvert.SerializeObject(self, OTPConverter.Settings);
        }
    }
}