using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using AfricasTalkingSDK;

namespace AfricasTalkingSDK
{
    class Simple
    {

        public void Test()
        {
            AfricasTalking.Initialize("sandbox", "123");
            SMSService sms = AfricasTalking.GetService("SMSService");
            Console.WriteLine(sms);
        }
    }
}
