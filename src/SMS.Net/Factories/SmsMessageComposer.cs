namespace SMS.Net.Factories
{
    using SMS.Net.Channel;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// a factory for creating the mail message.
    /// </summary>
    public class SmsMessageComposer
    {
        private PhoneNumber _from;
        private Priority _priority;
        private string _bodyContent;

        private readonly HashSet<PhoneNumber> _to;
        private readonly HashSet<ChannelData> _channelData;

        internal SmsMessageComposer()
        {
            _priority = Priority.Normal;
            _to = new HashSet<PhoneNumber>();
            _channelData = new HashSet<ChannelData>();
        }

        /// <summary>
        /// set the message body content.
        /// </summary>
        /// <param name="messageContent">the message body content</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
        public SmsMessageComposer WithContent(string messageContent)
        {
            _bodyContent = messageContent;
            return this;
        }

        /// <summary>
        /// add the sender phone number.
        /// </summary>
        /// <param name="phoneNumber">the sender phone number.</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
        public SmsMessageComposer From(string phoneNumber)
            => From(new PhoneNumber(phoneNumber));

        /// <summary>
        /// add the sender phone number.
        /// </summary>
        /// <param name="phoneNumber">the sender phone number.</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining.</returns>
        public SmsMessageComposer From(PhoneNumber phoneNumber)
        {
            _from = phoneNumber;
            return this;
        }

        /// <summary>
        /// add the recipient phone number.
        /// </summary>
        /// <param name="phoneNumber">recipient phone number.</param>
        /// <param name="delimiter">if the phone number is a list of phone numbers you can supply the delimiter of the numbers., by default is set to ";"</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining</returns>
        public SmsMessageComposer To(string phoneNumber, char delimiter = ';')
        {
            if (phoneNumber is null)
                throw new ArgumentNullException(nameof(phoneNumber));

            if (phoneNumber == string.Empty)
                throw new AggregateException("the given phone number is empty.");

            if (delimiter == default)
                delimiter = ';';

            // try to split the email address
            var phoneNumbers = phoneNumber.Split(delimiter);

            // if we have only one email
            if (phoneNumbers.Length == 1)
            {
                _to.Add(new PhoneNumber(phoneNumber));
                return this;
            }

            // add the email address
            foreach (var number in phoneNumbers)
                _to.Add(new PhoneNumber(number));

            return this;
        }

        /// <summary>
        /// add the recipient phone number.
        /// </summary>
        /// <param name="mailAddress">recipient phone number.</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining</returns>
        public SmsMessageComposer To(params PhoneNumber[] mailAddress)
        {
            foreach (var address in mailAddress)
                _to.Add(address);

            return this;
        }

        /// <summary>
        /// set the message priority to High.
        /// </summary>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining</returns>
        public SmsMessageComposer WithHighPriority()
        {
            _priority = Priority.High;
            return this;
        }

        /// <summary>
        /// set the message priority to Low.
        /// </summary>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining</returns>
        public SmsMessageComposer WithLowPriority()
        {
            _priority = Priority.Low;
            return this;
        }

        /// <summary>
        /// set the message priority to Normal.
        /// </summary>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining</returns>
        public SmsMessageComposer WithNormalPriority()
        {
            _priority = Priority.Normal;
            return this;
        }

        /// <summary>
        /// add the data to be passed to the delivery channel.
        /// </summary>
        /// <param name="key">the data key.</param>
        /// <param name="value">the data value.</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining</returns>
        public SmsMessageComposer PassChannelData(string key, object value)
            => PassChannelData(new ChannelData(key, value));

        /// <summary>
        /// add the data to be passed to the delivery channel.
        /// </summary>
        /// <param name="data">the data instance.</param>
        /// <returns>Instance of <see cref="SmsMessageComposer"/> to enable fluent chaining</returns>
        public SmsMessageComposer PassChannelData(params ChannelData[] data)
        {
            foreach (var item in data)
                _channelData.Add(item);

            return this;
        }

        /// <summary>
        /// build the <see cref="SmsMessage"/> instance.
        /// </summary>
        /// <returns>Instance of <see cref="SmsMessage"/>.</returns>
        public SmsMessage Build()
            => new SmsMessage(_priority, _bodyContent, _from, _to, _channelData);
    }
}
