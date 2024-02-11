namespace SMS.Net
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// the SMS.net DI builder.
    /// </summary>
    public class SmsNetBuilder
    {
        /// <summary>
        /// create an instance of <see cref="SmsNetBuilder"/>.
        /// </summary>
        /// <param name="serviceCollection">the services collection instance.</param>
        /// <param name="configuration">the SMS service option</param>
        internal SmsNetBuilder(IServiceCollection serviceCollection, SmsServiceOptions configuration)
        {
            Configuration = configuration;
            ServiceCollection = serviceCollection;
        }

        /// <summary>
        /// Get the service collection.
        /// </summary>
        public IServiceCollection ServiceCollection { get; }

        /// <summary>
        /// Get the SMS service options.
        /// </summary>
        public SmsServiceOptions Configuration { get; }
    }
}
