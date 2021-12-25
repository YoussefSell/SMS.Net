namespace SMS.Net
{
    using SMS.Net.Channel.Avochato;
    using SMS.Net.Factories;
    using System;

    /// <summary>
    /// the extensions methods over the <see cref="SmsServiceFactory"/> factory.
    /// </summary>
    public static class AvochatoSmsServiceFactoryExtensions
    {
        /// <summary>
        /// add the Avochato channel to be used with your SMS service.
        /// </summary>
        /// <param name="builder">the <see cref="SmsServiceFactory"/> instance.</param>
        /// <param name="authId">Set your Avochato accessKey.</param>
        /// <param name="authSecret">Set your Avochato authSecret.</param>
        /// <returns>instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
        public static SmsServiceFactory UseAvochato(this SmsServiceFactory builder, string authId, string authSecret)
           => builder.UseAvochato(op => { op.AuthId = authId; op.AuthSecret = authSecret; });

        /// <summary>
        /// add the Avochato channel to be used with your SMS service.
        /// </summary>
        /// <param name="builder">the <see cref="SmsServiceFactory"/> instance.</param>
        /// <param name="config">the configuration builder instance.</param>
        /// <returns>instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
        public static SmsServiceFactory UseAvochato(this SmsServiceFactory builder, Action<AvochatoSmsDeliveryChannelOptions> config)
        {
            // load the configuration
            var configuration = new AvochatoSmsDeliveryChannelOptions();
            config(configuration);

            // validate the configuration
            configuration.Validate();

            // add the Edp to the SMSs service factory
            builder.UseChannel(new AvochatoSmsDeliveryChannel(null, configuration));

            return builder;
        }
    }
}
