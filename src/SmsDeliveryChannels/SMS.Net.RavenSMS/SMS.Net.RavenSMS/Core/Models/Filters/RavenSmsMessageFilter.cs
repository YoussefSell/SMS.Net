namespace SMS.Net.Channel.RavenSMS.Domain.Models.Filters;

/// <summary>
/// the filter for retrieving the <see cref="RavenSmsMessage"/>.
/// </summary>
public class RavenSmsMessageFilter : FilterOptions
{
    public RavenSmsMessageFilter()
    {
        OrderBy = nameof(RavenSmsMessage.SentOn);
        SortDirection = SortDirection.Descending;
        Status = RavenSmsMessageStatus.None;

        To = new HashSet<string>();
        Clients = new HashSet<string>();
        ExcludeStatus = new HashSet<RavenSmsMessageStatus>();
    }

    /// <summary>
    /// Get or set the creation start date filter.
    /// </summary>
    public string? StartDate { get; set; }

    /// <summary>
    /// Get or set the creation end date filter.
    /// </summary>
    public string? EndDate { get; set; }

    /// <summary>
    /// Get or set the priority filter.
    /// </summary>
    public Priority? Priority { get; set; }

    /// <summary>
    /// Get or set the status filter.
    /// </summary>
    public RavenSmsMessageStatus Status { get; set; }
    
    /// <summary>
    /// status to exclude
    /// </summary>
    public IEnumerable<RavenSmsMessageStatus> ExcludeStatus { get; set; }

    /// <summary>
    /// Get or set the list of recipients filter.
    /// </summary>
    public IEnumerable<string> To { get; set; }

    /// <summary>
    /// Get or set the list of ravenSMS clients filter.
    /// </summary>
    public IEnumerable<string> Clients { get; set; }
}