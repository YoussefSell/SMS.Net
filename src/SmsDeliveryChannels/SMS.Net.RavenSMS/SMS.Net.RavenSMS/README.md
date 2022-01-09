# SMS.Net.RavenSMS - SMS delivery channel

send SMS messages using RavenSMS.

to get started first install
- **[SMS.Net.RavenSMS](https://www.nuget.org/packages/SMS.Net.RavenSMS/):** `Install-Package SMS.Net.RavenSMS`.  

if you're using Dependency Injection than install 
- **[SMS.Net.RavenSMS.DependencyInjection](https://www.nuget.org/packages/SMS.Net.RavenSMS.DependencyInjection/):** `Install-Package SMS.Net.RavenSMS.DependencyInjection`.  

##### Setup
in order to use the RavenSMS channel, you call the `UseRavenSMS()` method and pass the username and password.

```csharp
// register RavenSMS channel with SmsServiceFactory
SmsServiceFactory.Instance
    .UseRavenSMS(userName: "your-RavenSMS-userName", password:  "your-RavenSMS-password")
    .Create();

// register RavenSMS channel with Dependency Injection
services.AddSMSNet(RavenSMSSmsDeliveryProvider.Name)
    .UseRavenSMS(userName: "your-RavenSMS-userName", password:  "your-RavenSMS-password");
```

##### Custom channel data
RavenSMS channel allows you to pass the following data with the message instance

```csharp
// create the message
var message = SmsMessage.Compose()
    
    // to pass a custom username to be used instead the one configured in the options.
    .UseUserName("your-RavenSMS-userName")
    
    // to pass a custom password to be used instead the one configured in the options.
    .UsePassword("your-RavenSMS-password")
    
    // to pass a custom accountSID to be used instead the one configured in the options.
    .UseAccountSID("your-RavenSMS-account-sid")
    
    // this options that exist in the CreateMessageOptions object in RavenSMS
    .SetPathAccountSid("string value")
    .SetMessagingServiceSid("string value")
    .SetMediaUrl(new List<Uri> {})
    .SetStatusCallback(new Uri())
    .SetApplicationSid("string value")
    .SetMaxPrice(1.2)
    .SetProvideFeedback(true)
    .SetAttempt(10)
    .SetValidityPeriod(10)
    .SetForceDelivery(true)
    .SetSmartEncoded(true)
    .SetPersistentAction(new List<string>{})
    .SetSendAsMms(true);
```
