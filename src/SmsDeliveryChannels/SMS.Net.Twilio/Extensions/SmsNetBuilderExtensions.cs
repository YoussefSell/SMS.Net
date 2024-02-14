namespace SMS.Net;

/// <summary>
/// the extensions methods over the <see cref="SmsNetBuilder"/>.
/// </summary>
public static class SmsNetBuilderExtensions
{
    /// <summary>
    /// add the Twilio channel to be used with your SMS service.
    /// </summary>
    /// <param name="builder">the <see cref="SmsNetBuilder"/> instance.</param>
    /// <param name="username">Set your Twilio username.</param>
    /// <param name="password">Set your Twilio password.</param>
    /// <returns>instance of <see cref="SmsNetBuilder"/> to enable methods chaining.</returns>
    public static SmsNetBuilder UseTwilio(this SmsNetBuilder builder, string username, string password)
        => builder.UseTwilio(username, password, null);

    /// <summary>
    /// add the Twilio channel to be used with your SMS service.
    /// </summary>
    /// <param name="builder">the <see cref="SmsNetBuilder"/> instance.</param>
    /// <param name="username">Set your Twilio username.</param>
    /// <param name="password">Set your Twilio password.</param>
    /// <param name="accountSID">Set your Twilio account SID.</param>
    /// <returns>instance of <see cref="SmsNetBuilder"/> to enable methods chaining.</returns>
    public static SmsNetBuilder UseTwilio(this SmsNetBuilder builder, string username, string password, string? accountSID)
       => builder.UseTwilio(op => { op.Username = username; op.Password = password; op.AccountSID = accountSID; });

    /// <summary>
    /// add the Twilio channel to be used with your SMS service.
    /// </summary>
    /// <param name="builder">the SMSNet builder instance.</param>
    /// <param name="config">the configuration builder instance.</param>
    /// <returns>instance of <see cref="SmsNetBuilder"/> to enable methods chaining.</returns>
    public static SmsNetBuilder UseTwilio(this SmsNetBuilder builder, Action<TwilioSmsDeliveryChannelOptions> config)
    {
        // load the configuration
        var configuration = new TwilioSmsDeliveryChannelOptions();
        config(configuration);

        // validate the configuration
        configuration.Validate();

        builder.Services.AddSingleton((s) => configuration);
        builder.Services.AddScoped<ISmsDeliveryChannel, TwilioSmsDeliveryChannel>();
        builder.Services.AddScoped<ITwilioSmsDeliveryChannel, TwilioSmsDeliveryChannel>();

        return builder;
    }
}
