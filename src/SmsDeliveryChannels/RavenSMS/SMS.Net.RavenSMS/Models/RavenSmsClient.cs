namespace SMS.Net.Channel.RavenSMS
{
    using System;

    /// <summary>
    /// a class that defines a client that is used for sending SMS messages.
    /// </summary>
    public class RavenSmsClient
    {
        /// <summary>
        /// Get or set the id of the client.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or set for the client.
        /// </summary>
        public string Name { get; set; } = default!;
        
        /// <summary>
        /// Get or set a description for the client.
        /// </summary>
        public string Description { get; set; } = default!;

        /// <summary>
        /// Get or set the date
        /// </summary>
        public DateTimeOffset CreatedOn { get; set; }
    }
}