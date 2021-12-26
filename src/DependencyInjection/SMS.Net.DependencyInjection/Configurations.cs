namespace Microsoft.Extensions.DependencyInjection
{
    using SMS.Net;
    using System;

    /// <summary>
    /// the Configurations class
    /// </summary>
    public static class Configurations
    {
        /// <summary>
        /// add the SMS.Net services and configuration.
        /// </summary>
        /// <param name="serviceCollection">the service collection instant</param>
        /// <param name="defaultEdpName">name of the default edp to be used.</param>
        public static SmsNetBuilder AddSMSNet(this IServiceCollection serviceCollection, string defaultEdpName)
            => AddSMSNet(serviceCollection, options => options.DefaultDeliveryChannel = defaultEdpName);

        /// <summary>
        /// add the SMS.Net services and configuration.
        /// </summary>
        /// <param name="serviceCollection">the service collection instant</param>
        /// <param name="config">the configuration initializer.</param>
        public static SmsNetBuilder AddSMSNet(this IServiceCollection serviceCollection, Action<SmsServiceOptions> config)
        {
            if (config is null)
                throw new ArgumentNullException(nameof(config));

            // load the configuration
            var configuration = new SmsServiceOptions();
            config(configuration);

            serviceCollection.AddSingleton((s) => configuration);
            serviceCollection.AddScoped<ISmsService, SmsService>();

            // register the countries service
            return new SmsNetBuilder(serviceCollection, configuration);
        }
    }
}
