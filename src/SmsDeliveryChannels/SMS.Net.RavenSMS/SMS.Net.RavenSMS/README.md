# SMS.Net.Channel.RavenSMS - SMS delivery channel

RavenSMS is a custom developed channel that utilizes our phones to send sms messages without the need for buying a subscription from services like Twilio and MessageBird etc.

the idea behind RavenSMS has raised when i found that most SMS delivery channels don't offer a great testing experience. the testing budgets is very low on most of the services, Twilio for example gives you only 15$ to be used for testing which i don't find enough. also the sending limitation, most services restrict you you to send SMS messages to only a specific test phone number.

so i thought, i have a phone subscription with unlimited SMS messages to send, why not use my phone to send the messages. and RavenSMS has born.

## How it works?

there are two main component in the RavenSMS architecture:

- server: responsible for managing and sending messages, and producasting events to the clients.
- client: is the receiver of the events, which is your phone with RavenSMS app installed on it.

when you send a message with SMS.NET it will be forwarded to RavenSMS channel then the message will be queued for immediate delivery or after a delay. than when it time to send the message a websocket command is sent to the client (your phone) using SignalR with the message details and the message will be sent.

## Getting started

RavenSMS is just a channel for SMS.NET, it built on top of the abstraction provided to you by the SMS.NET library, so that if you want to switch back another channel it just a mater of modifying the configurations.

RavenSMS is built on top of ASP.NET Core 6, so keep that in mind if your project is not compatible with dotnet 6, it not going to work.

### 1. configure RavenSMS server:

so let's start with configuring our RavenSMS server, to get started first install in your ASP core project the RavenSMS package.

- **[SMS.Net.Channel.RavenSMS](https://www.nuget.org/packages/SMS.Net.Channel.RavenSMS/):** `Install-Package SMS.Net.Channel.RavenSMS`.

after installing it we need to add RavenSMS to SMS.NET configuration

```csharp
// add SMS.Net configuration
services.AddSMSNet(options =>
{
    options.PauseSending = false;
    options.DefaultFrom = new PhoneNumber("+212100000009");
    options.DefaultDeliveryChannel = RavenSmsDeliveryChannel.Name;
})
.UseRavenSMS();
```

and that it, now if you run the project and navigate to localhost:{port}/RavenSMS you will will see the RavenSMS dashboard.  
[insert RavenSMS dashboard screenshot]

### 2. Configure RavenSMS client:

now the next step is to configure the client, to do that install the RavenSMS application on the phone that you will be using as the client.

**Note**: you can download the latest version of the app APK from the [release page](https://github.com/YoussefSell/SMS.Net/releases).

after you install the app on the phone, navigate to **localhost:{port}/RavenSMS/Clients**, you will see a client that has been added by default, click on setup options:  
[insert RavenSMS clients page screenshot]

now you will see the setup page of the client, with a QR code:  
[insert RavenSMS client setup page screenshot]

open the app on your phone, and scan the QR code. once it scanned you phone will be connected to the server, to verify, navigate to **localhost:{port}/RavenSMS/Clients** and you will see the status of the client has changed to **Connected**.
[insert RavenSMS clients page with phone new status screenshot]

now let's send a message to make sure that everything is working fine. to do that navigate to **localhost:{port}/RavenSMS/Messages** and click on **Compose**,  
[insert RavenSMS messages page screenshot]

fill in the options and click on send. now if your phone has credits to send the message, you will receive the message and status of the message will be set to **Sent**, if not the status will be set to **Failed**

for a full documentation, please check the [Wiki page](https://github.com/YoussefSell/SMS.Net/wiki).
