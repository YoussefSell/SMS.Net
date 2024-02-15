namespace SMS.Net.Channel.Twilio.Test;

public class TwilioSmsDeliveryChannelShould
{
    static readonly string TEST_FROM_PHONE_NUMBER = EnvVariable.Load("SMS_NET_TWILIO_FROM_PHONE_NUMBER");
    static readonly string TEST_TO_PHONE_NUMBER = EnvVariable.Load("SMS_NET_TEST_TO_PHONE_NUMBER");
    static readonly string TEST_USERNAME = EnvVariable.Load("SMS_NET_TWILIO_USERNAME");
    static readonly string TEST_PASSWORD = EnvVariable.Load("SMS_NET_TWILIO_PASSWORD");

    [Fact]
    public void ThrowIfOptionsIsNull()
    {
        // arrange
        TwilioSmsDeliveryChannelOptions? options = null;

        // assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            // act
            new TwilioSmsDeliveryChannel(options!);
        });
    }

    [Fact]
    public void ThrowIfOptionsNotValid_AllIsNull()
    {
        // arrange
        var options = new TwilioSmsDeliveryChannelOptions();

        // assert
        Assert.Throws<RequiredOptionValueNotSpecifiedException<TwilioSmsDeliveryChannelOptions>>(() =>
        {
            // act
            new TwilioSmsDeliveryChannel(options);
        });
    }

    [Fact]
    public void ThrowIfOptionsNotValid_Username_IsNull()
    {
        // arrange
        var options = new TwilioSmsDeliveryChannelOptions()
        {
            Password = TEST_PASSWORD
        };

        // assert
        Assert.Throws<RequiredOptionValueNotSpecifiedException<TwilioSmsDeliveryChannelOptions>>(() =>
        {
            // act
            new TwilioSmsDeliveryChannel(options);
        });
    }

    [Fact]
    public void ThrowIfOptionsNotValid_Username_IsEmpty()
    {
        // arrange
        var options = new TwilioSmsDeliveryChannelOptions()
        {
            Username = "",
            Password = TEST_PASSWORD
        };

        // assert
        Assert.Throws<RequiredOptionValueNotSpecifiedException<TwilioSmsDeliveryChannelOptions>>(() =>
        {
            // act
            new TwilioSmsDeliveryChannel(options);
        });
    }

    [Fact]
    public void ThrowIfOptionsNotValid_Password_IsNull()
    {
        // arrange
        var options = new TwilioSmsDeliveryChannelOptions()
        {
            Username = TEST_USERNAME
        };

        // assert
        Assert.Throws<RequiredOptionValueNotSpecifiedException<TwilioSmsDeliveryChannelOptions>>(() =>
        {
            // act
            new TwilioSmsDeliveryChannel(options);
        });
    }

    [Fact]
    public void ThrowIfOptionsNotValid_Password_IsEmpty()
    {
        // arrange
        var options = new TwilioSmsDeliveryChannelOptions()
        {
            Password = "",
            Username = TEST_USERNAME
        };

        // assert
        Assert.Throws<RequiredOptionValueNotSpecifiedException<TwilioSmsDeliveryChannelOptions>>(() =>
        {
            // act
            new TwilioSmsDeliveryChannel(options);
        });
    }

    [Fact]
    public void CreateSmsMessageFromMessage()
    {
        // arrange
        var channel = new TwilioSmsDeliveryChannel(new TwilioSmsDeliveryChannelOptions()
        {
            Password = TEST_PASSWORD,
            Username = TEST_USERNAME
        });

        var message = SmsMessage.Compose()
            .From(TEST_FROM_PHONE_NUMBER)
            .To(TEST_TO_PHONE_NUMBER)
            .WithContent("this is a test")
            .Build();

        // act
        var mailMessage = channel.CreateMessage(message);

        // assert
        Assert.Equal(message.From.ToString(), mailMessage.From.ToString());
        Assert.Equal(message.To.ToString(), mailMessage.To.ToString());
        Assert.Equal(message.Body, mailMessage.Body);
    }

    [Fact]
    public void CreateSmsMessageFromMessageWithCustomData()
    {
        // arrange
        var channel = new TwilioSmsDeliveryChannel(new TwilioSmsDeliveryChannelOptions()
        {
            Password = TEST_PASSWORD,
            Username = TEST_USERNAME
        });

        var expectedPersistentAction = new List<string> { "value_1" };
        var expectedStatusCallback = new Uri("https://example.com/webhook");
        var expectedMediaUrl = new List<Uri> { new("https://example.com/logo.png") };

        var messageComposer = SmsMessage.Compose()
            .To(TEST_TO_PHONE_NUMBER)
            .From(TEST_FROM_PHONE_NUMBER)
            .WithContent("this is a test")

            // attach custom data
            .SetAttempt(1)
            .SetMaxPrice(12.2m)
            .SetSendAsMMS(true)
            .SetValidityPeriod(1)
            .SetSmartEncoded(true)
            .SetForceDelivery(true)
            .SetProvideFeedback(true)
            .SetMediaUrl(expectedMediaUrl)
            .SetStatusCallback(expectedStatusCallback)
            .SetApplicationSid("application_sid_value")
            .SetPersistentAction(expectedPersistentAction)
            .SetMessagingServiceSid("messaging_service_sid_value");

        var message = messageComposer.Build();

        // act
        var mailMessage = channel.CreateMessage(message);

        // assert
        Assert.True(mailMessage.SendAsMms);
        Assert.Equal(1, mailMessage.Attempt);
        Assert.True(mailMessage.SmartEncoded);
        Assert.True(mailMessage.ForceDelivery);
        Assert.True(mailMessage.ProvideFeedback);
        Assert.Equal(12.2m, mailMessage.MaxPrice);
        Assert.Equal(1, mailMessage.ValidityPeriod);
        Assert.Equal(expectedMediaUrl, mailMessage.MediaUrl);
        Assert.Equal(expectedStatusCallback, mailMessage.StatusCallback);
        Assert.Equal("application_sid_value", mailMessage.ApplicationSid);
        Assert.Equal(expectedPersistentAction, mailMessage.PersistentAction);
        Assert.Equal("messaging_service_sid_value", mailMessage.MessagingServiceSid);
    }

    [Fact(Skip = "no auth keys")]
    public void SendEmail()
    {
        // arrange
        var channel = new TwilioSmsDeliveryChannel(new TwilioSmsDeliveryChannelOptions()
        {
            Password = TEST_PASSWORD,
            Username = TEST_USERNAME
        });

        var message = SmsMessage.Compose()
            .From(TEST_FROM_PHONE_NUMBER)
            .To(TEST_TO_PHONE_NUMBER)
            .WithContent("this is a test")
            .Build();

        // act
        var result = channel.Send(message);

        // assert
        Assert.True(result.IsSuccess);
    }
}