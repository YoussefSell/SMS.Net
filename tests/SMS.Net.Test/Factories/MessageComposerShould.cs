namespace SMS.Test.Factories;

/// <summary>
/// the test class for the <see cref="SmsMessageComposer"/>
/// </summary>
public class MessageComposerShould
{
    [Fact]
    public void CreateMessageWithAllProps()
    {
        // arrange
        var composser = SmsMessage.Compose()
            .WithHighPriority()
            .To("+212625415254")
            .From("+212625415254")
            .WithContent("this the message content")
            .WithCustomData("key", "value");

        // act
        var message = composser.Build();

        // assert
        Assert.Equal("this the message content", message.Body);

        Assert.Equal("+212625415254", message.To.ToString());
        Assert.Equal("+212625415254", message.From.ToString());

        Assert.Equal(1, message.ChannelData.Count);
        Assert.Equal("key", message.ChannelData.First().Key);
        Assert.Equal("value", message.ChannelData.First().Value);

        Assert.Equal(Priority.High, message.Priority);
    }

    #region Message "Content" value test

    [Fact]
    public void AddMessageBodyContent()
    {
        // arrange
        var composser = SmsMessage.Compose().To("+212625415254");

        // act
        var message = composser
            .WithContent("this the message content")
            .Build();

        // assert
        Assert.Equal("this the message content", message.Body);
    }

    #endregion

    #region Message "From" value tests

    [Fact]
    public void CreateMessageWithFrom_FromString()
    {
        // arrange
        var composser = SmsMessage.Compose()
            .WithContent("this the message content")
            .To("+212625415254");

        var expected = "+212625415254";

        // act
        var message = composser
            .From("+212625415254")
            .Build();

        // assert
        Assert.Equal(expected, message.From.ToString());
    }

    [Fact]
    public void CreateMessageWithFrom_FromPhoneNumber()
    {
        // arrange
        var composser = SmsMessage.Compose()
            .WithContent("this the message content")
            .To(new PhoneNumber("test@sms.net"));

        var expected = "+212625415254";

        // act
        var message = composser
            .From(new PhoneNumber("+212625415254"))
            .Build();

        // assert
        Assert.Equal(expected, message.From.ToString());
    }

    [Fact]
    public void CreateMessageWithFrom_FromString_WithSpaces()
    {
        // arrange
        var composser = SmsMessage.Compose()
            .WithContent("this the message content")
            .To("+212625415254");

        // act
        var message = composser
            .From("        +212625415255       ")
            .Build();

        // assert
        Assert.Equal("+212625415255", message.From.ToString());
    }

    #endregion

    #region Message "To" value tests

    [Fact]
    public void ThrowExceptionIfNoToSms()
    {
        // arrange
        var composser = SmsMessage.Compose()
            .WithContent("this the message content");

        // act

        // assert
        Assert.Throws<ArgumentNullException>(() => composser.Build());
    }

    [Fact]
    public void CreateMessageWithTo_FromString()
    {
        // arrange
        var composser = SmsMessage.Compose().WithContent("this the message content");
        var expected = "+212625415254";

        // act
        var message = composser
            .To("+212625415254")
            .Build();

        // assert
        Assert.Equal(expected, message.To.ToString());
    }

    [Fact]
    public void CreateMessageWithTo_FromPhoneNumber()
    {
        // arrange
        var composser = SmsMessage.Compose().WithContent("this the message content");
        var expected = "+212625415256";

        // act
        var message = composser
            .To(new PhoneNumber("+212625415256"))
            .Build();

        // assert
        Assert.Equal(expected, message.To.ToString());
    }

    [Fact]
    public void CreateMessageWithTo_FromString_WithSpaces()
    {
        // arrange
        var composser = SmsMessage.Compose()
            .WithContent("this the message content");

        // act
        var message = composser
            .To("        +212625415255       ")
            .Build();

        // assert
        Assert.Equal("+212625415255", message.To.ToString());
    }

    #endregion

    #region Message "ChannelData" value tests

    [Fact]
    public void CreateMessageWithChannelData_FromKeyValue()
    {
        // arrange
        var composser = SmsMessage.Compose()
            .WithContent("this the message content")
            .To("+212625415254");

        // act
        var message = composser
            .WithCustomData("key", "value")
            .Build();

        // assert
        Assert.Equal(1, message.ChannelData.Count);
        Assert.Equal("key", message.ChannelData.First().Key);
        Assert.Equal("value", message.ChannelData.First().Value);
    }

    [Fact]
    public void CreateMessageWithChannelData_FromInstance()
    {
        // arrange
        var composser = SmsMessage.Compose()
            .WithContent("this the message content")
            .To("+212625415254");

        // act
        var message = composser
            .WithCustomData(new ChannelData("key", "value"))
            .Build();

        // assert
        Assert.Equal(1, message.ChannelData.Count);
        Assert.Equal("key", message.ChannelData.First().Key);
        Assert.Equal("value", message.ChannelData.First().Value);
    }

    [Fact]
    public void CreateMessageWithChannelData_FromListInstance()
    {
        // arrange 
        var composser = SmsMessage.Compose()
            .WithContent("this the message content")
            .To("+212625415254");

        // act
        var message = composser
            .WithCustomData(new ChannelData("key1", "value"), new ChannelData("key2", "value"))
            .Build();

        // assert
        Assert.Equal(2, message.ChannelData.Count);
        Assert.Equal("key1", message.ChannelData.First().Key);
        Assert.Equal("value", message.ChannelData.First().Value);
        Assert.Equal("key2", message.ChannelData.Skip(1).First().Key);
        Assert.Equal("value", message.ChannelData.Skip(1).First().Value);
    }

    #endregion

    #region Message Priority tests

    [Fact]
    public void MarkMessageWithHighPriority()
    {
        // arrange
        var composser = SmsMessage.Compose()
            .WithContent("this the message content")
            .To("+212625415254");

        // act
        var message = composser
            .WithHighPriority()
            .Build();

        // assert
        Assert.Equal(Priority.High, message.Priority);
    }

    [Fact]
    public void MarkMessageWithLowPriority()
    {
        // arrange
        var composser = SmsMessage.Compose()
            .WithContent("this the message content")
            .To("+212625415254");

        // act
        var message = composser
            .WithLowPriority()
            .Build();

        // assert
        Assert.Equal(Priority.Low, message.Priority);
    }

    [Fact]
    public void MarkMessageWithNormalPriority()
    {
        // arrange
        var composser = SmsMessage.Compose()
            .WithContent("this the message content")
            .To("+212625415254");

        // act
        var message = composser
            .WithNormalPriority()
            .Build();

        // assert
        Assert.Equal(Priority.Normal, message.Priority);
    }

    #endregion
}