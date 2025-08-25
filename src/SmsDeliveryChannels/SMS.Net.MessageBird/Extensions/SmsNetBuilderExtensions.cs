namespace SMS.Net;

/// <summary>
/// the extensions methods over the <see cref="SmsNetBuilder"/>.
/// </summary>
public static class SmsNetBuilderExtensions
{
    /// <summary>
    /// add the MessageBird channel to be used with your SMS service.
    /// </summary>
    /// <param name="builder">the <see cref="SmsNetBuilder"/> instance.</param>
    /// <param name="accessKey">Set your MessageBird accessKey.</param>
    /// <returns>instance of <see cref="SmsNetBuilder"/> to enable methods chaining.</returns>
    public static SmsNetBuilder UseMessageBird(this SmsNetBuilder builder, string accessKey)
       => builder.UseMessageBird(op => op.AccessKey = accessKey);


    /// <summary>
    /// add the MessageBird channel to be used with your sms service.
    /// </summary>
    /// <param name="builder">the SmsNet builder instance.</param>
    /// <param name="config">the configuration builder instance.</param>
    /// <returns>instance of <see cref="SmsNetBuilder"/> to enable methods chaining.</returns>
    public static SmsNetBuilder UseMessageBird(this SmsNetBuilder builder, Action<MessageBirdSmsDeliveryChannelOptions> config)
    {
        // load the configuration
        var configuration = new MessageBirdSmsDeliveryChannelOptions();
        config(configuration);

        // validate the configuration
        configuration.Validate();

        builder.Services.AddSingleton((s) => configuration);
        builder.Services.AddScoped<ISmsDeliveryChannel, MessageBirdSmsDeliveryChannel>();
        builder.Services.AddScoped<IMessageBirdSmsDeliveryChannel, MessageBirdSmsDeliveryChannel>();

        return builder;
    }
}
