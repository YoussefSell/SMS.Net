# SMS.Net.Twilio - Email delivery channel [EDP]

send SMSs using Twilio.

to get started first install
- **[SMS.Net.Twilio](https://www.nuget.org/packages/SMS.Net.Twilio/):** `Install-Package SMS.Net.Twilio`.  

if you're using Dependency Injection than install 
- **[SMS.Net.Twilio.DependencyInjection](https://www.nuget.org/packages/SMS.Net.Twilio.DependencyInjection/):** `Install-Package SMS.Net.Twilio.DependencyInjection`.  

##### Setup
in order to use the Twilio EDP, you call the `UseTwilio()` method and pass the api key and server id.

```csharp
// register Twilio EDP with SmsServiceFactory
SmsServiceFactory.Instance
    .UseTwilio(apiKey: "your-Twilio-api-key", serverId: 15478)
    .Create();

// register Twilio EDP with Dependency Injection
services.AddEmailNet(TwilioEmailDeliveryProvider.Name)
    .UseTwilio(apiKey: "your-Twilio-api-key", serverId: 15478);
```

##### Custom EDP data
Twilio EDP allows you to pass the folowwing data with the message instance

```csharp
// create the message
var message = Message.Compose()
    
    // to pass a custom message id
    .SetMessageId("your-message-id")
    
    // to pass a custom mailing id
    .SetMailingId("your-mailing-Id")
    
    // to pass a defrent api key
    .UseCustomApiKey("you-custom-api-key")
    
    // to pass a defrent server id
    .UseCustomServerId(15874);
```
