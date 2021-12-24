namespace SMS.Net.Factories
{
    using Channel;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// the SMS service factory used to generate an instance of <see cref="SmsService"/>
    /// </summary>
    public partial class SmsServiceFactory
    {
        /// <summary>
        /// get an instance of the <see cref="SmsServiceFactory"/>
        /// </summary>
        public static readonly SmsServiceFactory Instance = new SmsServiceFactory();

        /// <summary>
        /// set the options of the SMS service.
        /// </summary>
        /// <param name="options">the SMS option initializer.</param>
        /// <returns>instance of <see cref="SmsServiceFactory"/> to enable method chaining.</returns>
        public SmsServiceFactory UseOptions(Action<SmsServiceOptions> options)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            // set the SMS options and validate it.
            options(_options);
            _options.Validate();

            return this;
        }

        /// <summary>
        /// set the <see cref="ISmsChannel"/> to be used by the SMS service.
        /// </summary>
        /// <param name="channel">the <see cref="ISmsChannel"/> instance</param>
        /// <returns>instance of <see cref="SmsServiceFactory"/> to enable method chaining.</returns>
        public SmsServiceFactory UseChannel(ISmsChannel channel)
        {
            if (channel is null)
                throw new ArgumentNullException(nameof(channel));

            _channels.Add(channel);

            return this;
        }

        /// <summary>
        /// create the SMS service instance.
        /// </summary>
        /// <returns>instance of <see cref="SmsService"/></returns>
        public ISmsService Create() => new SmsService(_channels, _options);
    }

    /// <summary>
    /// partial part of <see cref="SmsServiceFactory"/>
    /// </summary>
    public partial class SmsServiceFactory
    {
        private readonly SmsServiceOptions _options = new SmsServiceOptions();
        private readonly HashSet<ISmsChannel> _channels = new HashSet<ISmsChannel>();

        private SmsServiceFactory() { }
    }
}
