namespace SMS.Net.Channel.RavenSMS.Models.Filters
{
    /// <summary>
    /// the filter for retrieving the <see cref="RavenSmsMessage"/>.
    /// </summary>
    public  class RavenSmsMessageFilter : FilterOptions
    {
        public RavenSmsMessageFilter()
        {
            To = new HashSet<string>();
            From = new HashSet<string>();
            Clients = new HashSet<Guid>();
            Status = new HashSet<RavenSmsMessageStatus>();
        }

        /// <summary>
        /// Get or set the creation start date filter.
        /// </summary>
        public DateTimeOffset? StartDate { get; set; }

        /// <summary>
        /// Get or set the creation end date filter.
        /// </summary>
        public DateTimeOffset? EndDate { get; set; }

        /// <summary>
        /// Get or set the priority filter.
        /// </summary>
        public Priority? Priority { get; set; }

        /// <summary>
        /// Get or set the status filter.
        /// </summary>
        public IEnumerable<RavenSmsMessageStatus> Status { get; set; }

        /// <summary>
        /// Get or set the list of recipients filter.
        /// </summary>
        public IEnumerable<string> To { get; set; }

        /// <summary>
        /// Get or set the list of senders filter.
        /// </summary>
        public IEnumerable<string> From { get; set; }

        /// <summary>
        /// Get or set the list of ravenSMS clients filter.
        /// </summary>
        public IEnumerable<Guid?> Clients { get; set; }
    }
}
