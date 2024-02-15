# SMS.Net.Twilio - SMS delivery channel

send SMS messages using Twilio.

to get started first install
- **[SMS.Net.Twilio](https://www.nuget.org/packages/SMS.Net.Twilio/):** `Install-Package SMS.Net.Twilio`.  

##### Setup
in order to use the Twilio channel, you call the `UseTwilio()` method and pass the username and password.

```csharp
// register Twilio channel with SmsServiceFactory
SmsServiceFactory.Instance
    .UseTwilio(userName: "your-Twilio-userName", password:  "your-Twilio-password")
    .Create();

// register Twilio channel with Dependency Injection
services.AddSMSNet(TwilioSmsDeliveryProvider.Name)
    .UseTwilio(userName: "your-Twilio-userName", password:  "your-Twilio-password");
```

##### Custom channel data
Twilio channel allows you to pass the following data with the message instance

```csharp
// create the message
var message = SmsMessage.Compose()
    
    // to pass a custom username to be used instead the one configured in the options.
    .UseUserName("your-Twilio-userName")
    
    // to pass a custom password to be used instead the one configured in the options.
    .UsePassword("your-Twilio-password")
    
    // to pass a custom accountSID to be used instead the one configured in the options.
    .UseAccountSID("your-Twilio-account-sid")
    
    // this options that exist in the CreateMessageOptions object in Twilio
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
