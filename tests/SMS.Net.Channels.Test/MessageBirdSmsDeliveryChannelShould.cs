namespace SMS.Net.Channel.MessageBird.Test
{
    using SMS.Net.Channel.MessageBird;
    using SMS.Net.Exceptions;
    using System;
    using Xunit;

    public class MessageBirdSmsDeliveryChannelShould
    {
        static readonly string TEST_FROM_PHONE_NUMBER = EnvVariable.Load("SMS_NET_MESSAGEBIRD_FROM_PHONE_NUMBER");
        static readonly string TEST_TO_PHONE_NUMBER = EnvVariable.Load("SMS_NET_TEST_TO_PHONE_NUMBER");
        static readonly string TEST_ACCESSKEY = EnvVariable.Load("SMS_NET_MESSAGEBIRD_ACCESSKEY");

        [Fact]
        public void ThorwIfOptionsIsNull()
        {
            // arrange
            MessageBirdSmsDeliveryChannelOptions? options = null;

            // assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // act
                new MessageBirdSmsDeliveryChannel(options);
            });
        }

        [Fact]
        public void ThorwIfOptionsNotValid_AllIsNull()
        {
            // arrange
            var options = new MessageBirdSmsDeliveryChannelOptions();

            // assert
            Assert.Throws<RequiredOptionValueNotSpecifiedException<MessageBirdSmsDeliveryChannelOptions>>(() =>
            {
                // act
                new MessageBirdSmsDeliveryChannel(options);
            });
        }

        [Fact]
        public void ThorwIfOptionsNotValid_Password_IsNull()
        {
            // arrange
            var options = new MessageBirdSmsDeliveryChannelOptions()
            {
            };

            // assert
            Assert.Throws<RequiredOptionValueNotSpecifiedException<MessageBirdSmsDeliveryChannelOptions>>(() =>
            {
                // act
                new MessageBirdSmsDeliveryChannel(options);
            });
        }

        [Fact]
        public void ThorwIfOptionsNotValid_Password_IsEmpty()
        {
            // arrange
            var options = new MessageBirdSmsDeliveryChannelOptions()
            {
                AccessKey = "",
            };

            // assert
            Assert.Throws<RequiredOptionValueNotSpecifiedException<MessageBirdSmsDeliveryChannelOptions>>(() =>
            {
                // act
                new MessageBirdSmsDeliveryChannel(options);
            });
        }

        [Fact(Skip = "no auth keys")]
        public void SendEmail()
        {
            // arrange
            var channel = new MessageBirdSmsDeliveryChannel(new MessageBirdSmsDeliveryChannelOptions()
            {
                AccessKey = TEST_ACCESSKEY
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
}