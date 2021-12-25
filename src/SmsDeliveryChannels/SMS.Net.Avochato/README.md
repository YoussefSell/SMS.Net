# SMS.Net.Avochato - SMS delivery channel

send SMS messages using Avochato.

to get started first install
- **[SMS.Net.Avochato](https://www.nuget.org/packages/SMS.Net.Avochato/):** `Install-Package SMS.Net.Avochato`.  

if you're using Dependency Injection than install 
- **[SMS.Net.Avochato.DependencyInjection](https://www.nuget.org/packages/SMS.Net.Avochato.DependencyInjection/):** `Install-Package SMS.Net.Avochato.DependencyInjection`.  

##### Setup
in order to use the Avochato channel, you call the `UseAvochato()` method and pass the username and password.

```csharp
// register Avochato channel with SmsServiceFactory
SmsServiceFactory.Instance
    .UseAvochato(authId: "your-Avochato-authId", authSecret: "your-Avochato-authSecret")
    .Create();

// register Avochato channel with Dependency Injection
services.AddSMSNet(AvochatoSmsDeliveryProvider.Name)
    .UseAvochato(authId: "your-Avochato-authId", authSecret: "your-Avochato-authSecret");
```

##### Custom channel data
Avochato channel allows you to pass the following data with the message instance

```csharp
// create the message
var message = SmsMessage.Compose()
    
    // to pass a custom authId to be used instead the one configured in the options.
    .UseAuthKey("your-Avochato-authId")

    // to pass a custom authSecret to be used instead the one configured in the options.
    .UseAuthSecret("your-Avochato-authSecret")

    // this options that exist in the Message request
    .SetMarkAddressed(true)
    .SetMediaUrl(new Uri(""))
    .SetTags(new List<string> {})
    .SetStatusCallback(new Uri(""));
```
