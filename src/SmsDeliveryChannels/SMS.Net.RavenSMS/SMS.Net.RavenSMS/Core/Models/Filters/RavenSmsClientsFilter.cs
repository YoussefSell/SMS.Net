namespace SMS.Net.Channel.RavenSMS.Models.Filters;

/// <summary>
/// the filter for retrieving the <see cref="RavenSmsClientsFilter"/>.
/// </summary>
public class RavenSmsClientsFilter : FilterOptions
{
    public RavenSmsClientsFilter()
    {
        phoneNumbers = new HashSet<string>();
        Status = RavenSmsClientStatus.None;
    }

    /// <summary>
    /// Get or set the status filter.
    /// </summary>
    public RavenSmsClientStatus Status { get; set; }

    /// <summary>
    /// Get or set the list of recipients filter.
    /// </summary>
    public IEnumerable<string> phoneNumbers { get; set; }
}
