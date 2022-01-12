namespace SMS.Net.Test.Factories
{
    using SMS.Net.Channel;
    using SMS.Net.Exceptions;
    using SMS.Net.Factories;
    using Moq;
    using System.Net.Mail;
    using Xunit;

    public class SmsServiceFactoryShould
    {
        private const string _channel_name = "channel";
        private readonly ISmsDeliveryChannel _channel;

        public SmsServiceFactoryShould()
        {
            var channelMock = new Mock<ISmsDeliveryChannel>();
            channelMock.Setup(e => e.Name).Returns(_channel_name);
            channelMock.Setup(e => e.Send(It.IsAny<SmsMessage>())).Returns(SmsSendingResult.Success(_channel_name));
            _channel = channelMock.Object;
        }

        [Fact]
        public void CreateEmailServiceWithOptionsAndSmtpEdp()
        {
            // arrange
            var factorty = SmsServiceFactory.Instance;
            var defaultEmail = new PhoneNumber("+212625415254");

            // act
            var service = factorty
                .UseOptions(options =>
                {
                    options.PauseSending = false;
                    options.DefaultFrom = defaultEmail;
                    options.DefaultDeliveryChannel = _channel_name;
                })
                .UseChannel(_channel)
                .Create() as SmsService;

            // assert
            if (service is not null)
            {
                Assert.Single(service.Channels);
                Assert.Equal(_channel_name, service.DefaultChannel.Name);
                Assert.Equal(defaultEmail, service.Options.DefaultFrom);
            }
        }

        [Fact]
        public void ThrowIfOptionsNotValid()
        {
            // arrange
            var factorty = SmsServiceFactory.Instance;
            var defaultEmail = new PhoneNumber("+212625415254");

            // assert
            Assert.Throws<RequiredOptionValueNotSpecifiedException<SmsServiceOptions>>(() =>
            {
                // act
                factorty
                    .UseOptions(options =>
                    {
                        options.PauseSending = false;
                        options.DefaultFrom = defaultEmail;
                        options.DefaultDeliveryChannel = null;
                    });
            });
        }
    }
}
