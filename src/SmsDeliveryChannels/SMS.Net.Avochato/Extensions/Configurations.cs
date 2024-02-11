namespace SMS.Net
{
    using Microsoft.Extensions.DependencyInjection;
    using SMS.Net.Channel;
    using SMS.Net.Channel.Avochato;
    using System;

    /// <summary>
    /// the Configurations class
    /// </summary>
    public static class Configurations
    {
        /// <summary>
        /// add the Avochato channel to be used with your SMS service.
        /// </summary>
        /// <param name="builder">the <see cref="SmsNetBuilder"/> instance.</param>
        /// <param name="authId">Set your Avochato accessKey.</param>
        /// <param name="authSecret">Set your Avochato authSecret.</param>
        /// <returns>instance of <see cref="SmsNetBuilder"/> to enable methods chaining.</returns>
        public static SmsNetBuilder UseAvochato(this SmsNetBuilder builder, string authId, string authSecret)
           => builder.UseAvochato(op => { op.AuthId = authId; op.AuthSecret = authSecret; });


        /// <summary>
        /// add the Avochato channel to be used with your email service.
        /// </summary>
        /// <param name="builder">the emailNet builder instance.</param>
        /// <param name="config">the configuration builder instance.</param>
        /// <returns>instance of <see cref="SmsNetBuilder"/> to enable methods chaining.</returns>
        public static SmsNetBuilder UseAvochato(this SmsNetBuilder builder, Action<AvochatoSmsDeliveryChannelOptions> config)
        {
            // load the configuration
            var configuration = new AvochatoSmsDeliveryChannelOptions();
            config(configuration);

            // validate the configuration
            configuration.Validate();

            builder.ServiceCollection.AddSingleton((s) => configuration);
            builder.ServiceCollection.AddHttpClient<ISmsDeliveryChannel, AvochatoSmsDeliveryChannel>();
            builder.ServiceCollection.AddHttpClient<IAvochatoSmsDeliveryChannel, AvochatoSmsDeliveryChannel>();

            return builder;
        }
    }
}
