namespace SMS.Net
{
    using SMS.Net.Channel.MessageBird;
    using SMS.Net.Factories;
    using System;

    /// <summary>
    /// the extensions methods over the <see cref="SmsServiceFactory"/> factory.
    /// </summary>
    public static class MessageBirdSmsServiceFactoryExtensions
    {
        /// <summary>
        /// add the MessageBird channel to be used with your SMS service.
        /// </summary>
        /// <param name="builder">the <see cref="SmsServiceFactory"/> instance.</param>
        /// <param name="accessKey">Set your MessageBird accessKey.</param>
        /// <returns>instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
        public static SmsServiceFactory UseMessageBird(this SmsServiceFactory builder, string accessKey)
           => builder.UseMessageBird(op => op.AccessKey = accessKey);

        /// <summary>
        /// add the MessageBird channel to be used with your SMS service.
        /// </summary>
        /// <param name="builder">the <see cref="SmsServiceFactory"/> instance.</param>
        /// <param name="config">the configuration builder instance.</param>
        /// <returns>instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
        public static SmsServiceFactory UseMessageBird(this SmsServiceFactory builder, Action<MessageBirdSmsDeliveryChannelOptions> config)
        {
            // load the configuration
            var configuration = new MessageBirdSmsDeliveryChannelOptions();
            config(configuration);

            // validate the configuration
            configuration.Validate();

            // add the channel to the SMSs service factory
            builder.UseChannel(new MessageBirdSmsDeliveryChannel(configuration));

            return builder;
        }
    }
}
