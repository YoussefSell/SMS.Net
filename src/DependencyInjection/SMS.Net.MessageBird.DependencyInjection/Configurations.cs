namespace Microsoft.Extensions.DependencyInjection
{
    using SMS.Net.Channel;
    using SMS.Net.Channel.MessageBird;
    using System;

    /// <summary>
    /// the Configurations class
    /// </summary>
    public static class Configurations
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
        /// add the MessageBird channel to be used with your email service.
        /// </summary>
        /// <param name="builder">the emailNet builder instance.</param>
        /// <param name="config">the configuration builder instance.</param>
        /// <returns>instance of <see cref="SmsNetBuilder"/> to enable methods chaining.</returns>
        public static SmsNetBuilder UseMessageBird(this SmsNetBuilder builder, Action<MessageBirdSmsDeliveryChannelOptions> config)
        {
            // load the configuration
            var configuration = new MessageBirdSmsDeliveryChannelOptions();
            config(configuration);

            // validate the configuration
            configuration.Validate();

            builder.ServiceCollection.AddSingleton((s) => configuration);
            builder.ServiceCollection.AddScoped<ISmsChannel, MessageBirdSmsDeliveryChannel>();
            builder.ServiceCollection.AddScoped<IMessageBirdSmsDeliveryChannel, MessageBirdSmsDeliveryChannel>();

            return builder;
        }
    }
}
