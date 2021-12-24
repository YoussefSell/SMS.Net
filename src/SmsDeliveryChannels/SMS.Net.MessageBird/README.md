# SMS.Net.MessageBird - SMS delivery channel

send SMS messages using MessageBird.

to get started first install
- **[SMS.Net.MessageBird](https://www.nuget.org/packages/SMS.Net.MessageBird/):** `Install-Package SMS.Net.MessageBird`.  

if you're using Dependency Injection than install 
- **[SMS.Net.MessageBird.DependencyInjection](https://www.nuget.org/packages/SMS.Net.MessageBird.DependencyInjection/):** `Install-Package SMS.Net.MessageBird.DependencyInjection`.  

##### Setup
in order to use the MessageBird channel, you call the `UseMessageBird()` method and pass the username and password.

```csharp
// register MessageBird channel with SmsServiceFactory
SmsServiceFactory.Instance
    .UseMessageBird(accessKey: "your-MessageBird-accessKey")
    .Create();

// register MessageBird channel with Dependency Injection
services.AddEmailNet(MessageBirdSmsDeliveryProvider.Name)
    .UseMessageBird(accessKey: "your-MessageBird-accessKey");
```

##### Custom channel data
MessageBird channel allows you to pass the following data with the message instance

```csharp
// create the message
var message = SmsMessage.Compose()
    
    // to pass a custom accessKey to be used instead the one configured in the options.
    .UseAccessKey("your-MessageBird-accessKey");
```
