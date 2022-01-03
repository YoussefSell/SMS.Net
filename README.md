# SMS.NET

[![](https://img.shields.io/github/license/YoussefSell/SMS.Net)](https://github.com/YoussefSell/SMS.Net/blob/master/LICENSE)
[![](https://img.shields.io/nuget/v/SMS.Net)](https://www.nuget.org/packages/SMS.Net/)
![Build](https://github.com/YoussefSell/SMS.Net/actions/workflows/ci.yml/badge.svg)

Send SMS message from your .Net application with a flexible solution that guarantee clean architectures, and access to different types of providers.

## Quick setup

to get started install the package using the [NuGet](https://www.nuget.org/packages/SMS.Net/) package manager `Install-Package SMS.Net`.

## Getting started

when you send an SMS message, there are three component that you will interact with:

- **SmsMessage**: the SMS message content to be sent.
- **SmsService**: the SMS service.
- **Channel**: the SMS Delivery Channel.

first we start by compose the SMS message, than we pass the message to the SMS service, than service will send the message using a pre-configured channel.

### 1. SmsMessage

the SmsMessage contain you message details, which includes the following:

- **From:** the phone number to be used as the sender.
- **To:** the phone number to be used as the recipient.
- **Body:** the SMS message text content.
- **Priority:** the priority of this SMS message.
- **ChannelData:** custom data to be passed to the delivery channel used for sending the sms message.

now let see how can we compose a message:

```csharp
var message = SmsMessage.Compose()
    .To("+212100000009")
    .WithContent("this is a test message")
    .WithHighPriority()
    .Build();
```

on the `SmsMessage` class you will find a method called `Compose()`, this method will give you a fluent API to compose your message so use the `'.'` and intellisense to see all the available function to compose you message, once you're done, call `Build()` to create an instance of the `SmsMessage`.

now we have a message let's try to send it.

### 2- Delivery Channels

Delivery Channels are what actually used to send the SMS messages under the hood, when you install SMS.Net you are required to install one of the available channels in order to be able to send the message.

the pre-built channel are provided as Nuget packages:

- **[SMS.Net.Twilio](https://www.nuget.org/packages/SMS.Net.Twilio/):** to send SMS messages using Twilio.
- **[SMS.Net.MessageBird](https://www.nuget.org/packages/SMS.Net.MessageBird/):** to send SMS messages using MessageBird.
- **[SMS.Net.Avochato](https://www.nuget.org/packages/SMS.Net.Avochato/):** to send SMS messages using Avochato.

and we will be adding more in the future, but if you want to create your own Channel you can follow this [tutorial](#) and you will learn how to build one.

### 3- SmsService

the SMS service is what you will be interacting with to send your messages, to create an instance of the service use can use the factory `SmsServiceFactory`

```csharp
var smsService = SmsServiceFactory.Instance
    .UseOptions(options =>
    {
        /*
         * if set to true we will not send any messages,
         * great if we don't want to send message while testing other functionalities
         */
        options.PauseSending = false;

        /* used to specify the default from to be used when sending the SMS messages */
        options.DefaultFrom = new PhoneNumber("+212100000009");

        /* set the default chanel to be used for sending the SMS messages */
        options.DefaultDeliveryChannel = TwilioSmsDeliveryChannel.Name;
    })
    // register the EDPs
    .UseTwilio("username", "password")
    .Create();
```

on the `SmsServiceFactory` class you will find a static property `Instance` that give you an instance of the factory, than you will have access to three methods on the factory:

- **UseOptions():** to configure the SMS service options.
- **UseChannel():** to register the delivery channels to be used for sending SMS messages.
- **Create():** to create an instance of the SmsService.

starting with `UseOptions()` you can configure the `SmsService` options, there are three options:

- **PauseSending:** to pause the sending of SMS messages, if set to true nothing will be sent.
- **DefaultFrom:** you can set the default Sender phone number, so that you don't have to do it each time on the message, note that if you have specified a Sender phone number on the message this value will be ignored.
- **DefaultDeliveryChannel:** specify the default channel that should be used to send the SMS messages, because you can configure multiple channels you should indicate which one you want to be used.

`UseChannel()` takes an instance of the Channel, like so: `UseChannel(new TwilioSmsDeliveryChannel(configuration))`, but you're not going to use this method, instead you will use the extension methods given to you by the channels as we seen on the example above, the Twilio channel has an extension method `UseTwilio()` that will allow you to register it.

finally `Create()` will simply create an instance of the `SmsService`.

you only need to create the SMS service once and reuse it in your app.

now you have an instance of the `SmsService` you can start sending SMS messages.

```csharp
// get the sms service
var smsService = SmsServiceFactory.Instance
    .UseOptions(options =>
    {
        options.PauseSending = false;
        options.DefaultFrom = new PhoneNumber("+212100000009");
        options.DefaultDeliveryChannel = TwilioSmsDeliveryChannel.Name;
    })
    .UseTwilio("username", "password")
    .Create();

// create the message
var message = SmsMessage.Compose()
    .To("+212100000009")
    .WithContent("this is a test message")
    .WithHighPriority()
    .Build();

// send the message
var result = smsService.Send(message);
```

## working with Dependency Injection

to register SMS.Net with DI we need to use [**SMS.Net.DependencyInjection**](https://www.nuget.org/packages/SMS.Net.DependencyInjection/) package, this package contains an extension method on the `IServiceCollection` interface that register the `SmsService` as a Scoped service.

once you have the package downloaded you can register SMS.Net like so:

```csharp
// add SMS.Net configuration
services.AddSMSNet(options =>
{
    options.PauseSending = false;
    options.DefaultFrom = new PhoneNumber("+212100000009");
    options.DefaultDeliveryChannel = TwilioSmsDeliveryChannel.Name;
})
.UseTwilio("username", "password");
```

then you can inject the SMS Service in your classes constructors using `ISmsService`

```csharp
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ISmsService _smsService;

    public IndexModel(ILogger<IndexModel> logger, ISmsService smsService)
    {
        _logger = logger;
        _smsService = smsService;
    }

    public void OnGet()
    {
        /* compose the sms message */
        var message = SmsMessage.Compose()
            .To("+212100000009")
            .WithContent("this is a test message")
            .WithHighPriority()
            .Build();

        /* send the message, this will use the default channel set in the option */
        var result = _smsService.Send(message);

        /* log the result */
        _logger.LogInformation("sent: {result}", result.IsSuccess);
    }
}
```
