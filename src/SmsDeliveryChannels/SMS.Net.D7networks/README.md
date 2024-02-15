# SMS.Net.D7networks - SMS delivery channel

send SMS messages using D7networks.

to get started first install
- **[SMS.Net.D7networks](https://www.nuget.org/packages/SMS.Net.D7networks/):** `Install-Package SMS.Net.D7networks`.   

##### Setup
in order to use the D7networks channel, you call the `UseD7networks()` method and pass the API Key.

```csharp
// register D7networks channel with SmsServiceFactory
SmsServiceFactory.Instance
    .UseD7networks(apiKey: "your-D7networks-api-key")
    .Create();

// register D7networks channel with Dependency Injection
services.AddSMSNet(D7networksSmsDeliveryProvider.Name)
    .UseD7networks(apiKey: "your-D7networks-api-key");
```

##### Custom channel data
D7networks channel allows you to pass the following data with the message instance

```csharp
// create the message
var message = SmsMessage.Compose()
    
    // to pass a custom API key to be used instead the one configured in the options.
    .UseApiKey("your-D7networks-api-key")

    // this options that exist in the Message request
    .SetReportUrl(new Uri(""))
    .SetOriginator("")
    .SetDataCoding(DataCoding.TEXT)
    .SetMessageType(MessageType.TEXT)
    .SetScheduleTime(DateTime.Now);
```
