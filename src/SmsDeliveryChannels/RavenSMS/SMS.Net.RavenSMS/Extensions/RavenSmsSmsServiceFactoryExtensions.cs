namespace SMS.Net
{
    using SMS.Net.Channel.RavenSMS;
    using SMS.Net.Factories;
    using System;

    /// <summary>
    /// the extensions methods over the <see cref="SmsServiceFactory"/> factory.
    /// </summary>
    public static class RavenSmsSmsServiceFactoryExtensions
    {
        /// <summary>
        /// add the RavenSMS channel to be used with your SMS service.
        /// </summary>
        /// <param name="builder">the <see cref="SmsServiceFactory"/> instance.</param>
        /// <param name="username">Set your RavenSMS username.</param>
        /// <param name="password">Set your RavenSMS password.</param>
        /// <returns>instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
        public static SmsServiceFactory UseRavenSMS(this SmsServiceFactory builder, string username, string password)
            => builder.UseRavenSMS(username, password, null);

        /// <summary>
        /// add the RavenSMS channel to be used with your SMS service.
        /// </summary>
        /// <param name="builder">the <see cref="SmsServiceFactory"/> instance.</param>
        /// <param name="username">Set your RavenSMS username.</param>
        /// <param name="password">Set your RavenSMS password.</param>
        /// <param name="accountSID">Set your RavenSMS account SID.</param>
        /// <returns>instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
        public static SmsServiceFactory UseRavenSMS(this SmsServiceFactory builder, string username, string password, string? accountSID)
           => builder.UseRavenSMS(op => { op.Username = username; op.Password = password; op.AccountSID = accountSID; });

        /// <summary>
        /// add the RavenSMS channel to be used with your SMS service.
        /// </summary>
        /// <param name="builder">the <see cref="SmsServiceFactory"/> instance.</param>
        /// <param name="config">the configuration builder instance.</param>
        /// <returns>instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
        public static SmsServiceFactory UseRavenSMS(this SmsServiceFactory builder, Action<RavenSmsDeliveryChannelOptions> config)
        {
            // load the configuration
            var configuration = new RavenSmsDeliveryChannelOptions();
            config(configuration);

            // validate the configuration
            configuration.Validate();

            // add the Edp to the SMSs service factory
            builder.UseChannel(new RavenSmsDeliveryChannel(configuration));

            return builder;
        }
    }
}
