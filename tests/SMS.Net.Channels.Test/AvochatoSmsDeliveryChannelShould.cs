namespace SMS.Net.Channel.Avochato.Test;

public class AvochatoSmsDeliveryChannelShould
{
    static readonly string TEST_FROM_PHONE_NUMBER = EnvVariable.Load("SMS_NET_AVOCHATO_FROM_PHONE_NUMBER");
    static readonly string TEST_TO_PHONE_NUMBER = EnvVariable.Load("SMS_NET_TEST_TO_PHONE_NUMBER");
    static readonly string TEST_AUTHSECRET = EnvVariable.Load("SMS_NET_AVOCHATO_AUTHSECRET");
    static readonly string TEST_AUTHID = EnvVariable.Load("SMS_NET_AVOCHATO_AUTHID");

    [Fact]
    public void ThrowIfOptionsIsNull()
    {
        // arrange
        AvochatoSmsDeliveryChannelOptions? options = null;

        // assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            // act
            new AvochatoSmsDeliveryChannel(null, options!);
        });
    }

    [Fact]
    public void ThrowIfOptionsNotValid_AllIsNull()
    {
        // arrange
        var options = new AvochatoSmsDeliveryChannelOptions();

        // assert
        Assert.Throws<RequiredOptionValueNotSpecifiedException<AvochatoSmsDeliveryChannelOptions>>(() =>
        {
            // act
            new AvochatoSmsDeliveryChannel(null, options);
        });
    }

    [Fact]
    public void ThrowIfOptionsNotValid_AuthSecret_IsNull()
    {
        // arrange
        var options = new AvochatoSmsDeliveryChannelOptions()
        {
            AuthId = TEST_AUTHID,
        };

        // assert
        Assert.Throws<RequiredOptionValueNotSpecifiedException<AvochatoSmsDeliveryChannelOptions>>(() =>
        {
            // act
            new AvochatoSmsDeliveryChannel(null, options);
        });
    }

    [Fact]
    public void ThrowIfOptionsNotValid_AuthSecret_IsEmpty()
    {
        // arrange
        var options = new AvochatoSmsDeliveryChannelOptions()
        {
            AuthId = TEST_AUTHID,
            AuthSecret = "",
        };

        // assert
        Assert.Throws<RequiredOptionValueNotSpecifiedException<AvochatoSmsDeliveryChannelOptions>>(() =>
        {
            // act
            new AvochatoSmsDeliveryChannel(null, options);
        });
    }

    [Fact]
    public void ThrowIfOptionsNotValid_Password_IsNull()
    {
        // arrange
        var options = new AvochatoSmsDeliveryChannelOptions()
        {
            AuthSecret = TEST_AUTHSECRET,
        };

        // assert
        Assert.Throws<RequiredOptionValueNotSpecifiedException<AvochatoSmsDeliveryChannelOptions>>(() =>
        {
            // act
            new AvochatoSmsDeliveryChannel(null, options);
        });
    }

    [Fact]
    public void ThrowIfOptionsNotValid_AuthId_IsEmpty()
    {
        // arrange
        var options = new AvochatoSmsDeliveryChannelOptions()
        {
            AuthId = "",
            AuthSecret = TEST_AUTHSECRET,
        };

        // assert
        Assert.Throws<RequiredOptionValueNotSpecifiedException<AvochatoSmsDeliveryChannelOptions>>(() =>
        {
            // act
            new AvochatoSmsDeliveryChannel(null, options);
        });
    }

    [Fact]
    public void CreateSmsMessageFromMessage()
    {
        // arrange
        var channel = new AvochatoSmsDeliveryChannel(null, new AvochatoSmsDeliveryChannelOptions()
        {
            AuthId = TEST_AUTHID,
            AuthSecret = TEST_AUTHSECRET,
        });

        var message = SmsMessage.Compose()
            .From(TEST_FROM_PHONE_NUMBER)
            .To(TEST_TO_PHONE_NUMBER)
            .WithContent("this is a test")
            .Build();

        // act
        var mailMessage = channel.CreateMessage(message);

        // assert
        Assert.Equal(message.From.ToString(), mailMessage.From);
        Assert.Equal(message.To.ToString(), mailMessage.Phone);
        Assert.Equal(message.Body, mailMessage.Message);
    }

    [Fact]
    public void CreateSmsMessageFromMessageWithCustomData()
    {
        // arrange
        var channel = new AvochatoSmsDeliveryChannel(null, new AvochatoSmsDeliveryChannelOptions()
        {
            AuthId = TEST_AUTHID,
            AuthSecret = TEST_AUTHSECRET,
        });

        var expectedTags = new List<string> { "tag_1", "tag_2" };
        var expectedStatusCallback = new Uri("https://example.com/webhook");
        var expectedMediaUrl = new Uri("https://example.com/logo.png");

        var messageComposer = SmsMessage.Compose()
            .To(TEST_FROM_PHONE_NUMBER)
            .From(TEST_TO_PHONE_NUMBER)
            .WithContent("this is a test")

            // attach custom data
            .SetTags(expectedTags)
            .SetMarkAddressed(true)
            .SetMediaUrl(expectedMediaUrl)
            .SetStatusCallback(expectedStatusCallback);

        var message = messageComposer.Build();

        // act
        var mailMessage = channel.CreateMessage(message);

        // assert
        Assert.True(mailMessage.MarkAddressed);
        Assert.Equal("tag_1,tag_2", mailMessage.Tags);
        Assert.Equal(expectedMediaUrl, mailMessage.MediaUrl);
        Assert.Equal(expectedStatusCallback, mailMessage.StatusCallback);
    }

    [Fact(Skip = "no auth keys")]
    public void SendSms()
    {
        // arrange
        var channel = new AvochatoSmsDeliveryChannel(null,  new AvochatoSmsDeliveryChannelOptions()
        {
            AuthId = TEST_AUTHID,
            AuthSecret = TEST_AUTHSECRET,
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