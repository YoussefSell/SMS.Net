namespace SMS.Net;

/// <summary>
/// the extensions methods over the <see cref="SmsServiceFactory"/> factory.
/// </summary>
public static class TwilioSmsServiceFactoryExtensions
{
    /// <summary>
    /// add the Twilio channel to be used with your SMS service.
    /// </summary>
    /// <param name="builder">the <see cref="SmsServiceFactory"/> instance.</param>
    /// <param name="authToken">Set your Twilio auth token.</param>
    /// <param name="accountSID">Set your Twilio account SID.</param>
    /// <returns>instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
    public static SmsServiceFactory UseTwilio(this SmsServiceFactory builder, string accountSID, string authToken)
       => builder.UseTwilio(op => { op.AuthToken = authToken; op.AccountSID = accountSID; });

    /// <summary>
    /// add the Twilio channel to be used with your SMS service.
    /// </summary>
    /// <param name="builder">the <see cref="SmsServiceFactory"/> instance.</param>
    /// <param name="config">the configuration builder instance.</param>
    /// <returns>instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
    public static SmsServiceFactory UseTwilio(this SmsServiceFactory builder, Action<TwilioSmsDeliveryChannelOptions> config)
    {
        // load the configuration
        var configuration = new TwilioSmsDeliveryChannelOptions();
        config(configuration);

        // validate the configuration
        configuration.Validate();

        // add the channel to the SMSs service factory
        builder.UseChannel(new TwilioSmsDeliveryChannel(configuration));

        return builder;
    }
}
