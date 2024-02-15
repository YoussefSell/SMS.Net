namespace SMS.Net.Channel.D7Networks.Test;

public class D7NetworksSmsDeliveryChannelShould
{
    static readonly string TEST_FROM_PHONE_NUMBER = EnvVariable.Load("SMS_NET_D7NETWORKS_FROM_PHONE_NUMBER");
    static readonly string TEST_TO_PHONE_NUMBER = EnvVariable.Load("SMS_NET_TEST_TO_PHONE_NUMBER");
    static readonly string TEST_APIKEY = EnvVariable.Load("SMS_NET_D7NETWORKS_APIKEY");

    [Fact]
    public void ThrowIfOptionsIsNull()
    {
        // arrange
        D7NetworksSmsDeliveryChannelOptions? options = null;

        // assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            // act
            new D7NetworksSmsDeliveryChannel(null, options!);
        });
    }

    [Fact]
    public void ThrowIfOptionsNotValid_AllIsNull()
    {
        // arrange
        var options = new D7NetworksSmsDeliveryChannelOptions();

        // assert
        Assert.Throws<RequiredOptionValueNotSpecifiedException<D7NetworksSmsDeliveryChannelOptions>>(() =>
        {
            // act
            new D7NetworksSmsDeliveryChannel(null, options);
        });
    }

    [Fact]
    public void ThrowIfOptionsNotValid_API_KEY_IsNull()
    {
        // arrange
        var options = new D7NetworksSmsDeliveryChannelOptions()
        {
            ApiKey = null,
        };

        // assert
        Assert.Throws<RequiredOptionValueNotSpecifiedException<D7NetworksSmsDeliveryChannelOptions>>(() =>
        {
            // act
            new D7NetworksSmsDeliveryChannel(null, options);
        });
    }

    [Fact]
    public void ThrowIfOptionsNotValid_API_KEY_IsEmpty()
    {
        // arrange
        var options = new D7NetworksSmsDeliveryChannelOptions()
        {
            ApiKey = "",
        };

        // assert
        Assert.Throws<RequiredOptionValueNotSpecifiedException<D7NetworksSmsDeliveryChannelOptions>>(() =>
        {
            // act
            new D7NetworksSmsDeliveryChannel(null, options);
        });
    }

    [Fact]
    public void CreateSmsMessageFromMessage()
    {
        // arrange
        var channel = new D7NetworksSmsDeliveryChannel(null, new D7NetworksSmsDeliveryChannelOptions()
        {
            ApiKey = TEST_APIKEY,
        });

        var message = SmsMessage.Compose()
            .From(TEST_FROM_PHONE_NUMBER)
            .To(TEST_TO_PHONE_NUMBER)
            .WithContent("this is a test")
            .Build();

        // act
        var mailMessage = channel.CreateMessage(message);

        // assert
        Assert.Equal(message.To.ToString(), mailMessage.Messages[0].Recipients[0]);
        Assert.Equal(message.Body, mailMessage.Messages[0].Content);
    }

    [Fact]
    public void CreateSmsMessageFromMessageWithCustomData()
    {
        // arrange
        var channel = new D7NetworksSmsDeliveryChannel(null, new D7NetworksSmsDeliveryChannelOptions()
        {
            ApiKey = TEST_APIKEY,
        });

        var expectedScheduleTime = DateTime.Now;
        var expectedReportUrl = new Uri("https://example.com/logo.png");
        var expectedScheduleTimeString = DateTime.Now.ToString("yyyy-MM-ddTHH:mmzzz");

        var messageComposer = SmsMessage.Compose()
            .To(TEST_FROM_PHONE_NUMBER)
            .From(TEST_TO_PHONE_NUMBER)
            .WithContent("this is a test")

            // attach custom data
            .SetOriginator("test")
            .SetDataCoding(DataCoding.TEXT)
            .SetReportUrl(expectedReportUrl)
            .SetMessageType(MessageType.TEXT)
            .SetScheduleTime(expectedScheduleTime);

        var message = messageComposer.Build();

        // act
        var mailMessage = channel.CreateMessage(message);

        // assert
        Assert.Equal("test", mailMessage.MessageGlobals!.Originator);
        Assert.Equal(DataCoding.TEXT, mailMessage.Messages[0].DataCoding);
        Assert.Equal(MessageType.TEXT, mailMessage.Messages[0].MessageType);
        Assert.Equal(expectedReportUrl, mailMessage.MessageGlobals!.ReportUrl);
        Assert.Equal(expectedScheduleTimeString, mailMessage.MessageGlobals!.ScheduleTime);
    }

    [Fact(Skip = "no auth keys")]
    public void SendEmail()
    {
        // arrange
        var channel = new D7NetworksSmsDeliveryChannel(null,  new D7NetworksSmsDeliveryChannelOptions()
        {
            ApiKey = TEST_APIKEY,
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