namespace SMS.Net.Test;

public class SmsServiceOptionsShould
{
    [Fact]
    public void ThrowIfFRequiredValueIsNotSpecified()
    {
        // arrange
        var options = new SmsServiceOptions() { DefaultDeliveryChannel = null! };

        // assert
        Assert.Throws<RequiredOptionValueNotSpecifiedException<SmsServiceOptions>>(() =>
        {
            // act
            options.Validate();
        });
    }
}
