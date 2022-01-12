namespace SMS.Net.Test.Services
{
    using SMS.Net.Channel;
    using SMS.Net.Exceptions;
    using Moq;
    using System;
    using Xunit;

    public class SmsServiceShould
    {
        private const string _channel_name_1 = "mock1_channel";
        private const string _channel_name_2 = "mock2_channel";
        private readonly ISmsDeliveryChannel _channel1;
        private readonly ISmsDeliveryChannel _channel2;

        public SmsServiceShould()
        {
            var channel1_mock = new Mock<ISmsDeliveryChannel>();
            channel1_mock.Setup(e => e.Name).Returns(_channel_name_1);
            channel1_mock.Setup(e => e.Send(It.IsAny<SmsMessage>())).Returns(SmsSendingResult.Success(_channel_name_1));
            _channel1 = channel1_mock.Object;

            var channel2_mock = new Mock<ISmsDeliveryChannel>();
            channel2_mock.Setup(e => e.Name).Returns(_channel_name_2);
            channel2_mock.Setup(e => e.Send(It.IsAny<SmsMessage>())).Returns(SmsSendingResult.Success(_channel_name_2));
            _channel2 = channel2_mock.Object;
        }

        [Fact]
        public void ThorwIfSmsDeliveryChannelNull()
        {
            // arrange
            var options = new SmsServiceOptions();

            // act

            // assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new SmsService(null, options);
            });
        }

        [Fact]
        public void ThorwIfSmsDeliveryChannelEmpty()
        {
            // arrange
            var options = new SmsServiceOptions();
            var edps = Array.Empty<ISmsDeliveryChannel>();

            // act

            // assert
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new SmsService(edps, options);
            });
        }

        [Fact]
        public void ThorwIfOptionsAreNull()
        {
            // arrange
            SmsServiceOptions? options = null;

            // act

            // assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new SmsService(new[] { _channel1 }, options);
            });
        }

        [Fact]
        public void ThorwIfOptionsAreNotValid()
        {
            // arrange
            var options = new SmsServiceOptions();

            // act

            // assert
            Assert.Throws<RequiredOptionValueNotSpecifiedException<SmsServiceOptions>>(() =>
            {
                _ = new SmsService(new[] { _channel1 }, options);
            });
        }

        [Fact]
        public void ThorwIfDefaultEdpNotFound()
        {
            // arrange
            var options = new SmsServiceOptions { DefaultDeliveryChannel = "not_exist_edp" };

            // act

            // assert
            Assert.Throws<SmsDeliveryChannelNotFoundException>(() =>
            {
                _ = new SmsService(new[] { _channel1 }, options);
            });
        }

        [Fact]
        public void SendMessageWithDefaultProvider()
        {
            // arrange
            var options = new SmsServiceOptions { DefaultDeliveryChannel = _channel_name_1 };
            var service = new SmsService(new[] { _channel1, _channel2 }, options);
            var message = SmsMessage.Compose()
                .From("+212625415254")
                .To("+212625415254")
                .WithContent("this the message content")
                .Build();

            // act
            var result = service.Send(message);

            // assert
            Assert.Equal(_channel_name_1, result.ChannelName);
        }

        [Fact]
        public void SendMessageWithProviderOfGivenName()
        {
            // arrange
            var options = new SmsServiceOptions { DefaultDeliveryChannel = _channel_name_1 };
            var service = new SmsService(new[] { _channel1, _channel2 }, options);
            var message = SmsMessage.Compose()
                .From("+212625415254")
                .To("+212625415254")
                .WithContent("this the message content")
                .Build();

            // act
            var result = service.Send(message, _channel_name_2);

            // assert
            Assert.Equal(_channel_name_2, result.ChannelName);
        }

        [Fact]
        public void ThrowIfGivenChannelNameForSendMessageNotExist()
        {
            // arrange
            var options = new SmsServiceOptions { DefaultDeliveryChannel = _channel_name_1 };
            var service = new SmsService(new[] { _channel1 }, options);
            var message = SmsMessage.Compose()
                .From("+212625415254")
                .To("+212625415254")
                .WithContent("this the message content")
                .Build();

            // assert
            Assert.Throws<SmsDeliveryChannelNotFoundException>(() =>
            {
                // act
                service.Send(message, _channel_name_2);
            });
        }

        [Fact]
        public void ThrowIfGivenChannelNameForSendMessageIsNull()
        {
            // arrange
            var options = new SmsServiceOptions { DefaultDeliveryChannel = _channel_name_1 };
            var service = new SmsService(new[] { _channel1 }, options);
            var message = SmsMessage.Compose()
                .From("+212625415254")
                .To("+212625415254")
                .WithContent("this the message content")
                .Build();

            // assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // act
                service.Send(message, edp_name: null);
            });
        }

        [Fact]
        public void SendMessageWithTheGivenProviderInstance()
        {
            // arrange
            var options = new SmsServiceOptions { DefaultDeliveryChannel = _channel_name_1 };
            var service = new SmsService(new[] { _channel1 }, options);
            var message = SmsMessage.Compose()
                .From("+212625415254")
                .To("+212625415254")
                .WithContent("this the message content")
                .Build();

            // act
            var result = service.Send(message, _channel2);

            // assert
            Assert.Equal(_channel_name_2, result.ChannelName);
        }

        [Fact]
        public void ThrowIfGivenEdpInstanceForSendIsNull()
        {
            // arrange
            var options = new SmsServiceOptions { DefaultDeliveryChannel = _channel_name_1 };
            var service = new SmsService(new[] { _channel1 }, options);
            var message = SmsMessage.Compose()
                .From("+212625415254")
                .To("+212625415254")
                .WithContent("this the message content")
                .Build();

            // assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // act
                _ = service.Send(message, edp: null);
            });
        }

        [Fact]
        public void ThrowOnSendWhenMessageInstanceIsNull()
        {
            // arrange
            var options = new SmsServiceOptions { DefaultDeliveryChannel = _channel_name_1 };
            var service = new SmsService(new[] { _channel1 }, options);

            // assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // act
                _ = service.Send(null);
            });
        }

        [Fact]
        public void UseDefaultSenderEmailSetInSmsServiceOptions()
        {
            // arrange
            var options = new SmsServiceOptions { DefaultDeliveryChannel = _channel_name_1, DefaultFrom = new PhoneNumber("+212625415254") };
            var service = new SmsService(new[] { _channel1 }, options);
            var message = SmsMessage.Compose()
                .To("+212625415254")
                .WithContent("this the message content")
                .Build();

            // act
            var result = service.Send(message);

            // assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void ThrowIfSenderEmailIsNotSetNeitherInOptionsOrMessage()
        {
            // arrange
            var options = new SmsServiceOptions { DefaultDeliveryChannel = _channel_name_1 };
            var service = new SmsService(new[] { _channel1 }, options);

            var message = SmsMessage.Compose()
                .To("+212625415254")
                .WithContent("this the message content")
                .Build();

            // assert
            Assert.Throws<ArgumentException>(() =>
            {
                // act
                _ = service.Send(message);
            });
        }

        [Fact]
        public void NotSendIfSendingIsPaused()
        {
            // arrange
            var options = new SmsServiceOptions { DefaultDeliveryChannel = _channel_name_1, PauseSending = true };
            var service = new SmsService(new[] { _channel1 }, options);
            var message = SmsMessage.Compose()
                .From("+212625415254")
                .To("+212625415254")
                .WithContent("this the message content")
                .Build();

            // act
            var result = service.Send(message);

            // assert
            Assert.True(result.GetMetaData<bool>(SmsSendingResult.MetaDataKeys.SendingPaused));
        }
    }
}
