# Official Africa's Talking C# API wrapper 

The Africa's Talking C# API wrapper provides convenient access to the Africa's Talking API from applications written in C#. With support for .NET45, .NET46 and .NET Standard 2.0. 

## Documentation 
Take a look at the [API docs here](http://docs.africastalking.com/) for more information. 

## Installation Options
1. #### Using Visual Studio IDE

+ On a new project, Navigate to the ***Solution Explorer*** tab within Visual Studio. 
+ Right-click on the ***References*** node and click on the *Manage Nuget Packages* from the resulting context menu. 
+ On the Nuget Package Manager window navigate to the ***Browse*** Tab. 
+ Key in **AfricasTalking.NET** and select version _1.1.411_ or higher. 
+ Click on the ***Install*** button and accept the licences to proceed. 

> For .NET Standard 2.0 projects yellow triangles may appear on your solution items,note that these are warnings due to deprecated support for some packages used by the wrapper.These will be reviewed in future releases,for now they will not affect the functionality of your project and can be safely ignored.Should there be a case where this package breaks your project kindly report the package via Nuget. 

![Install Package](ScreenShots/packageMgrInst.PNG) 


2. #### Using .NET CLI 

+ From the _command prompt/powershell window_ opened in your project directory, key in the following and press *Enter*. 
```powershell 
 dotnet add package AfricasTalking.NET --version 1.1.411
```
> Ensure you have the latest version of the package. Visit [Nuget](https://www.nuget.org/packages/AfricasTalking.NET/) for more info on the latest release of this package. 

3. #### Using Nuget Package Manger Console 

+ On your Nuget package manager console,key in the following and press *Enter* 
```powershell 
Install-Package AfricasTalking.NET -Version 1.1.41 
```
> Ensure you have the latest version of the package. Visit [Nuget](https://www.nuget.org/packages/AfricasTalking.NET/) for more info on the latest release of this package

## Usage 

+ To use this package ensure you add the following `using` statement to your project file: 
```csharp 
 using AfricasTalkingCS;
```

The package needs to be configured with your Africa's Talking username and API key (which you can get from the dashboard). 

```csharp  

var username = "YourUSERNAME";
var apiKey = "yourAPIKEY";
var env = "sandbox"; //Use this only if you need to use the sandbox environment,otherwise ignore. 

```
> Your default environment is **production**,hence you can use our gateway class as shown below

```csharp  

 var gateway = new AfricasTalkingGateway(username, apiKey);
  
```
> Otherwise, for sandbox;see below 

```csharp 
 
 var gateway = new AfricasTalkingGateway(username, apiKey, env); 
 
```
## Important: 
 If you register a callback URL with the API, always remember to acknowledge the receipt of any data it sends by responding with an HTTP `200`;  [Here's a sample application you can use to test a call-back url](https://github.com/TheBeachMaster/ATSamples/tree/master/paymentcallback.node) 
> For example in an ASP.NET Core or ASP.NET MVC Project

```csharp 
[HttpPost]
public ActionResult SomeCoolMethod(awesome,params)
{
   // Your awesome logic

   //If not using MVC5
   return new HttpStatusCodeResult(200);

   //If using MVC5
   return new HttpStatusCodeResult(HttpStatusCode.OK);  // OK = 200
} 
```

### SMS 
```csharp 
            try
            {
                var sms = gateway.SendMessage(recepients, msg);
                foreach (var res in sms["SMSMessageData"]["Recipients"])
                {
                    Console.WriteLine((string)res["number"] + ": ");
                    Console.WriteLine((string)res["status"] + ": ");
                    Console.WriteLine((string)res["messageId"] + ": ");
                    Console.WriteLine((string)res["cost"] + ": ");
                }
            }
            catch (AfricasTalkingGatewayException exception)
            {
                Console.WriteLine(exception);
            }
```

#### [Sending SMS](http://docs.africastalking.com/sms/sending) 

- `SendMessage(to,message,from,bulkSmsMode,options)` :  The following arguments are supplied to facilitate sending of messages via our APIs  

    - `to` : The recipient(s) expecting the message 
    - `message` : The SMS body. 
    - `from` :  (`Optional`) The Short-code or Alphanumeric ID that is associated with an Africa's Talking account.  
    - `bulkSmsMode` (`Optional`) : This parameter will be used by the Mobile Service Provider to determine who gets  billed for a message sent using a Mobile-Terminated Short-code. Must be set to  *1*  for Bulk SMS. .
    - `options` :   (`Optional`). Passed as _key-value_ pairs 
        -   `enque` : This parameter is used for Bulk SMS clients that would like deliver as many messages to the API before waiting for an Ack from the Telcos. If enabled, the API will store the messages in its databases and send them out asynchronously after responding to the request 
        -   `keyword` : This parameter is used for premium services. It is essential for subscription premium services.
        -   `linkId` : This parameter is used for premium services to send OnDemand messages. We forward the linkId to your application when the user send a message to your service. (Essential for premium subscription services) 
        -   `retryDurationInHours` : This parameter is used for premium messages. It specifies the number of hours your subscription message should be retried in case it's not delivered to the subscriber. (Essential for premium subscription services)

    ​

#### [Retrieving SMS](http://docs.africastalking.com/sms/fetchmessages)

> You can register a callback URL with us and we will forward any messages that are sent to your account the moment they arrive. 
> [Read more](http://docs.africastalking.com/sms/callback)

- `FetchMessages(lastReceivedId)` : Manually retrieve your messages.

    - `lastReceivedId` : This is the id of the message that you last processed. If this is your first call, pass in 0. `REQUIRED`


#### [Premium Subscriptions](http://docs.africastalking.com/subscriptions/create)

- `CreateSubscription(phoneNumber,shortCode,keyWord,checkoutToken)`:

    - `shortCode` : This is a premium short code mapped to your account `REQUIRED`
    - `keyWord` : Value is a premium keyword under the above short code and mapped to your account. `REQUIRED`
    - `phoneNumber`: The phoneNumber to be subscribed `REQUIRED`
    - `checkoutToken` :  This is a token used to validate the subscription request  `REQUIRED` 

     > If you have subscription products on your premium SMS short codes, you will need to configure a callback URL that we will invoke to notify you when users subscribe or unsubscribe from your products (currently supported on Safaricom).Visit [this link](http://docs.africastalking.com/subscriptions/callback) to learn more on how to setup a subscription callback  

> Example    - Creating Premium SMS subscription

```c#
            var username = "sandbox";
            var apikey = "KEY";
            var env = "sandbox";
            var gateway = new AfricasTalkingGateway(username, apikey, env);
            var shortCode = "NNNNN";
            var keyword = "keyword";
            var phoneNum = "+254XXXXXXXXX";
            var token = "Token";
            try
            {
                var response = gateway.CreateSubscription(phoneNum, shortCode, keyword, token);
                Console.WriteLine(response);
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("We hit a snag: " + e.StackTrace + ". " + e.Message);
                throw;
            }
```

> Example -Sending Premium SMS

```c#
            var username = "sandbox";
            var apikey = "KEY";
            var env = "sandbox";
            var gateway = new AfricasTalkingGateway(username, apikey, env);
            var opts = new Hashtable { ["keyword"] = "mykeyword" }; // ....
            var from = "NNNNN";
            var to = "+2547XXXXX,+2547XXXXY";
            var message = "Super Cool Message";
            try
            {
                var res = gateway.SendMessage(to, message, from, 1, opts); // Set Bulk to true
                Console.WriteLine(res);
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("Whoops: " + e.Message);
                throw;
            }
```



### [Airtime](http://docs.africastalking.com/airtime/sending)

```csharp
var airtimeTransaction = gateway.SendAirtime(airtimerecipients);
```
- `SendAirtime(recipients)`: 
    - `recipients`: Contains JSON objects containing the following keys
        - `phoneNumber`: Recipient of airtime
        - `amount`: Amount sent `>= 10 && <= 10K` with currency e.g `KES 100`


```csharp

            var username = "sandbox";
            var apikey = "MyAPIKEY";
            var airtimerecipients = @"{'phoneNumber':'+254XXXXXXXX','amount':'KES 250'}"; // Send any JSON object of n-Length
            var env = "sandbox";
            var gateway = new AfricasTalkingGateway(username, apikey, env);
            try
            {
                var airtimeTransaction = gateway.SendAirtime(airtimerecipients);
                Console.WriteLine(airtimeTransaction);
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("We ran into issues: " + e.StackTrace + ": " + e.Message);
            }
```

### [Payments](http://docs.africastalking.com/payments)

> Mobile Consumer To Business (C2B) functionality allows your application to receive payments that are initiated by a mobile subscriber.
> This is typically achieved by disctributing a PayBill or BuyGoods number (and optionally an account number) that clients can use to make payments from their mobile devices.
> [Read more](http://docs.africastalking.com/payments/mobile-c2b)


#### [Checkout](http://docs.africastalking.com/payments/mobile-checkout)

- `Checkout(productName,phoneNumber,currencyCode,amount,providerChannel,metadata)` :  Initiate Customer to Business (C2B) payments on a mobile subscriber's device. [More info](http://docs.africastalking.com/payments/mobile-checkout)

    - `productName`: Your Payment Product. `REQUIRED`

    - `phoneNumber`: The customer phone number (in international format; e.g. `25471xxxxxxx`). `REQUIRED`

    - `currencyCode`: 3-digit ISO format currency code (e.g `KES`, `USD`, `UGX` etc.) `REQUIRED`

    - `amount`: This is the amount. `REQUIRED`

    - `providerChannel`: Default provider channel details.For example `MPESA` or `Athena` for sandbox. 
      - **(Sandbox) :**  Note that for sandbox you'll have to manually create a channel that will be associated with `Athena`. This is the channel name that you will pass as an argument during a checkout.  Example after creating a channel you will have `Athena:channel_name` , pass `channel_name ` as the  _providerChannel_ . 

        ​

    - `metadata` : This value contains a map of any metadata that you would like us to associate with this request. You can use this field to send data that will map notifications to checkout requests, since we will include it when we send notifications once the checkout is complete. `(Optional)`

      ​

 > Example 
 ```csharp 
            var username = "sandbox";
            var environment = "sandbox";
            var apiKey = "";
            var productName = "coolproduct";
            var phoneNumber = "+254XXXXXXX";
            var currency = "KES";
            int amount = 35700;
            var channel = "mychannel";
    		var metadata = new Dictionary<string, string>
            {
                { "reason", "stuff" }
            };

            var gateway = new AfricasTalkingGateway(username, apiKey, environment);

            try
            {
                var checkout = gateway.Checkout(productName, phoneNumber, currency, amount, channel, 								   metadata);
                Console.WriteLine(checkout);
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("We ran into problems: " + e.Message);
            } 
 ```

#### [B2C](http://docs.africastalking.com/payments/mobile-b2c)


- `MobileB2C(productName,recepients)`:  Initiate payments to mobile subscribers from your payment wallet. [More info](http://docs.africastalking.com/payments/mobile-b2c)

    - `productName`: Your Payment Product. `REQUIRED`

    - `recipients`: A list of **up to 10** recipients info and metadata. Each recipient has:

        - `phoneNumber`: The payee phone number (in international format; e.g. `+25471xxxxxxx`). `REQUIRED`

        - `currencyCode`: 3-digit ISO format currency code (e.g `KES`, `USD`, `UGX` etc.) `REQUIRED`

        - `amount`: Payment amount. `REQUIRED`

        - `metadata`: Some optional data to associate with transaction. 

> Example 

```csharp  
 // Suppose a superhero unknowingly paid for a suit they did not like,and we want to refund them
            // or you want to pay your employees,staff etc...
            // Note::
            /*
             * Remember: In a live account ensure you have registered a credible B2C product and a callback url else these transactions will fail
             */

            // Developer Details
            string username = "sandbox";
            string environment = "sandbox";
            string apiKey = "apikey";
            string productName = "coolproduct";
            string currencyCode = "KES";

            // Recepient details,these can be retrieved from a db..or somewhere else then parsed...we'll keep it simple
            string hero1PhoneNum = "+254xxxxxxx";
            string hero2PhoneNum = "+254xxxxxxx";
            string hero1Name = "Batman";
            string hero2Name = "Superman";
            decimal hero1amount = 15000M;
            decimal hero2amount = 54000M;

            // We invoke our gateway
            var gateway = new AfricasTalkingGateway(username, apiKey, environment);

            // Let's create a bunch of people who'll be receiving the refund or monthly salary etc...
            var hero1 = new MobileB2CRecepient(hero1Name, hero1PhoneNum, currencyCode, hero1amount);

            // we can add metadata...like why we're paying them/refunding them etc...
            hero1.AddMetadata("reason", "Torn Suit");
            var hero2 = new MobileB2CRecepient(hero2Name, hero2PhoneNum, currencyCode, hero2amount);
            hero2.AddMetadata("reason", "Itchy Suit");

            // ....etc

            // Next we create a recepients list
            IList<MobileB2CRecepient> heroes = new List<MobileB2CRecepient>
            {
                hero1,
                hero2
            };

            // let's refund them so that they can keep saving the planet
            try
            {
                var response = gateway.MobileB2C(productName, heroes);
                Console.WriteLine(heroes);
                Console.WriteLine(response);
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("We ran into problems: " + e.StackTrace + e.Message);
            }
            Console.ReadLine();
        }
```


#### [B2B](http://docs.africastalking.com/payments/mobile-b2b)


- `MobileB2B(product,providerChannel,transfer,currency,transferAmount,channelReceiving,accountReceiving,b2Bmetadata)` :  Mobile Business To Business (B2B) APIs allow you to initiate payments TO businesses eg banks FROM your payment wallet. [More info](http://docs.africastalking.com/payments/mobile-b2b) 

    - `product` :  Your Payment Product as setup on your account. `REQUIRED`   
    - `providerChannel` :  This contains the payment provider that is facilitating this transaction.`REQUIRED` 
       Supported providers at the moment are:  

    ​         

    ```c#
    string provider = "Athena";
    // or 
    string provider = "MPESA";
    ```

    ​

    - `transfer`: This contains the payment provider that is facilitating this transaction. Supported providers at the moment are: `REQUIRED 

      ```c#
      string transferType = "BusinessToBusinessTransfer";
             // Transfer Type
                  /*
                   *BusinessBuyGoods
                   *BusinessPayBill
                   *DisburseFundsToBusiness
                   *BusinessToBusinessTransfer
                   */
      ```

      ​

    - `currency`: 3-digit ISO format currency code (e.g `KES`, `USD`, `UGX` etc.) `REQUIRED`

    - `channelReceiving`: This value contains the name or number of the channel that will receive payment by the provider. `REQUIRED`

    - `accountReceiving`: This value contains the account name used by the business to receive money on the provided destinationChannel. `REQUIRED`

    - `transferAmount`: Payment amount. `REQUIRED`

    - `b2Bmetadata`: Some optional data to associate with transaction. `REQUIRED`   



> Example 
```c# 
// Suppose we want to move money from our *awesomeproduct* to *coolproduct*
            /*
             * Remember to register  B2B products and callback urls:else these trasactions will fail
             */
            string username = "sandbox";
            string apiKey = "APIKEY";
            string productName = "awesomeproduct";
            string currencyCode = "KES";
            decimal amount = 15;
            string provider = "Athena";
            string destinationChannel = "mychannel"; // This is the channel that will be receiving the payment
            string destinationAccount = "coolproduct";

            // Transfer Type
            /*
             *BusinessBuyGoods
             *BusinessPayBill
             *DisburseFundsToBusiness
             *BusinessToBusinessTransfer
             */
            dynamic metadataDetails = new JObject();
            metadataDetails.shopName = "cartNumber";
            metadataDetails.Info = "Stuff";
            string transferType = "BusinessToBusinessTransfer";
            var gateWay = new AfricasTalkingGateway(username, apiKey);
            try
            {
                string response = gateWay.MobileB2B(
                    productName,
                    provider,
                    transferType,
                    currencyCode,
                    amount,
                    destinationChannel,
                    destinationAccount,
                    metadataDetails);
                Console.WriteLine(response);
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("Woopsies! We ran into issues: " + e.StackTrace + " : " + e.Message);
            }
 
```


