namespace Microsoft.Extensions.DependencyInjection
{
    using SMS.Net.Channel.RavenSMS;
    using System;

    /// <summary>
    /// the options for building the RavenSMS integration.
    /// </summary>
    public class RavenSmsBuilderOptions
    {
        /// <summary>
        /// create an instance of <see cref="RavenSmsBuilderOptions"/>
        /// </summary>
        /// <param name="builder"></param>
        internal RavenSmsBuilderOptions(SmsNetBuilder builder) => Builder = builder;

        /// <summary>
        /// the SMS builder instance
        /// </summary>
        public SmsNetBuilder Builder { get; }

        /// <summary>
        /// set the options values
        /// </summary>
        /// <param name="configuration">the options instance</param>
        internal void InitOptions(RavenSmsDeliveryChannelOptions configuration)
        {
            throw new NotImplementedException();
        }
    }
}
