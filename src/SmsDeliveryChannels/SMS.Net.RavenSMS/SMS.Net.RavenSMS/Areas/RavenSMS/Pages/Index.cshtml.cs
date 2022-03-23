namespace SMS.Net.Channel.RavenSMS.Pages;

/// <summary>
/// the dashboard pages
/// </summary>
public partial class DashboradPageModel
{
    /// <summary>
    /// the total of messages sent
    /// </summary>
    public long TotalSent { get; set; }

    /// <summary>
    /// the total of messages failed to sent
    /// </summary>
    public long TotalFailed { get; set; }

    /// <summary>
    /// the total of messages in the queue
    /// </summary>
    public long TotalInQueue { get; set; }

    /// <summary>
    /// the total of registered clients
    /// </summary>
    public long TotalClients { get; set; }
}

/// <summary>
/// partial part for <see cref="DashboradPageModel"/>
/// </summary>
public partial class DashboradPageModel
{
    public async Task OnGetAsync()
    {
        (TotalSent, TotalFailed, TotalInQueue) = await _messagesManager.MessagesCountsAsync();
        TotalClients = await _clientsManager.GetClientsCountAsync();
    }
}

/// <summary>
/// partial part for <see cref="DashboradPageModel"/>
/// </summary>
public partial class DashboradPageModel : BasePageModel
{
    private readonly IRavenSmsClientsManager _clientsManager;
    private readonly IRavenSmsMessagesManager _messagesManager;

    public DashboradPageModel(
        IRavenSmsClientsManager clientsManager,
        IRavenSmsMessagesManager messagesManager,
        IStringLocalizer<ClientSetupPageModel> localizer,
        ILogger<ClientSetupPageModel> logger)
        : base(localizer, logger)
    {
        _clientsManager = clientsManager;
        _messagesManager = messagesManager;
    }
}
