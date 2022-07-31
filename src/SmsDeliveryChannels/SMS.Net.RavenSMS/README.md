# SMS.Net.Channel.RavenSMS - SMS delivery channel

RavenSMS is a custom developed channel that utilizes our phones to send sms messages without the need for buying a subscription from services like Twilio and MessageBird etc.

for more details checkout the [project repository on Github](https://github.com/YoussefSell/RavenSMS)
a complete documentation of RavenSMS integration can be found on the [Wiki page](https://github.com/YoussefSell/RavenSMS/wiki)

## Getting started

RavenSMS is built on top of NET 6, so keep that in mind if your project is not compatible with .NET 6, it not going to work.

### 1. configure RavenSMS server:

so let's start with configuring our RavenSMS server, to get started first install the following packages:

- **[SMS.Net.Channel.RavenSMS](https://www.nuget.org/packages/SMS.Net.Channel.RavenSMS/):** `Install-Package SMS.Net.Channel.RavenSMS`.
- **[RavenSMS.Dashboard](https://www.nuget.org/packages/RavenSMS.Dashboard/):** `Install-Package RavenSMS.Dashboard` if you want to integrate RavenSMS dashboard.

when using RavenSMS with SMS.NET we need to register it using the `SmsNetBuilder`, so after installing it we need to add RavenSMS to SMS.NET configuration as follow:

```csharp
// add SMS.Net configuration
services.AddSMSNet(options =>
{
    options.PauseSending = false;
    options.DefaultFrom = new PhoneNumber("+212100000009");
    options.DefaultDeliveryChannel = RavenSmsDeliveryChannel.Name;
})
.UseRavenSMS(options => 
{
    config.UseInMemoryQueue();
    config.UseInMemoryStores();
    
    config.UseDashboard(); // if you installed the dashboard package
});
```

and that it, now if you run the project and navigate to localhost:{port}/RavenSMS you will see the RavenSMS dashboard.  

for complete documentation of how to configure RavenSMS check out the [Wiki page](https://github.com/YoussefSell/RavenSMS/wiki) on GitHub.

##### Custom channel data
RavenSMS channel allows you to pass the following data with the message instance

```csharp
// create the message
var message = SmsMessage.Compose()
    
    // to pass a custom delay before sending the message.
    .SendAfter(TimeSpan.FromSeconds(1));
```
