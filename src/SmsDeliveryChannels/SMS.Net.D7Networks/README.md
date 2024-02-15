# SMS.Net.D7Networks - SMS delivery channel

send SMS messages using D7Networks.

to get started first install
- **[SMS.Net.D7Networks](https://www.nuget.org/packages/SMS.Net.D7Networks/):** `Install-Package SMS.Net.D7Networks`.   

##### Setup
in order to use the D7Networks channel, you call the `UseD7Networks()` method and pass the API Key.

```csharp
// register D7Networks channel with SmsServiceFactory
SmsServiceFactory.Instance
    .UseD7Networks(apiKey: "your-d7networks-api-key")
    .Create();

// register D7Networks channel with Dependency Injection
services.AddSMSNet(D7NetworksSmsDeliveryProvider.Name)
    .UseD7Networks(apiKey: "your-d7networks-api-key");
```

##### Custom channel data
D7Networks channel allows you to pass the following data with the message instance

```csharp
// create the message
var message = SmsMessage.Compose()
    
    // to pass a custom API key to be used instead the one configured in the options.
    .UseApiKey("your-d7networks-api-key")

    // this options that exist in the Message request
    .SetReportUrl(new Uri(""))
    .SetOriginator("")
    .SetDataCoding(DataCoding.TEXT)
    .SetMessageType(MessageType.TEXT)
    .SetScheduleTime(DateTime.Now);
```
