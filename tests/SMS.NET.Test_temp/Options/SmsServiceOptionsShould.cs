namespace SMS.Net.Test
{
    using SMS.Net.Exceptions;
    using Xunit;

    public class SmsServiceOptionsShould
    {
        [Fact]
        public void ThrowIfFRequiredValueIsNotSpecified()
        {
            // arrange
            var options = new SmsServiceOptions() { DefaultDeliveryChannel = null };

            // assert
            Assert.Throws<RequiredOptionValueNotSpecifiedException<SmsServiceOptions>>(() =>
            {
                // act
                options.Validate();
            });
        }
    }
}
