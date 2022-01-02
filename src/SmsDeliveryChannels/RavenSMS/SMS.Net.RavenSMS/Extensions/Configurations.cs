namespace Microsoft.Extensions.DependencyInjection
{
    using SMS.Net.Channel;
    using SMS.Net.Channel.RavenSMS;
    using System;

    /// <summary>
    /// the Configurations class
    /// </summary>
    public static class Configurations
    {
        /// <summary>
        /// add the Twilio channel to be used with your SMS service.
        /// </summary>
        /// <param name="builder">the SMSNet builder instance.</param>
        /// <param name="config">the configuration builder instance.</param>
        /// <returns>instance of <see cref="SmsNetBuilder"/> to enable methods chaining.</returns>
        public static SmsNetBuilder UseRavenSMS(this SmsNetBuilder builder, Action<RavenSmsBuilderOptions> config)
        {
            // load the configuration
            var builderOptions = new RavenSmsBuilderOptions(builder);
            config(builderOptions);

            // validate the configuration
            var configuration = new RavenSmsDeliveryChannelOptions();
            builderOptions.InitOptions(configuration);
            configuration.Validate();

            builder.ServiceCollection.AddSingleton((s) => configuration);
            builder.ServiceCollection.AddScoped<ISmsChannel, RavenSmsDeliveryChannel>();
            builder.ServiceCollection.AddScoped<IRavenSmsDeliveryChannel, RavenSmsDeliveryChannel>();

            return builder;
        }
    }
}
