namespace SMS.Net.RavenSMS.Pages;

/// <summary>
/// the Clients add page
/// </summary>
public partial class ClientsAddPageModel
{
}

/// <summary>
/// partial part for <see cref="ClientsAddPageModel"/>
/// </summary>
public partial class ClientsAddPageModel : BasePageModel
{
    private readonly IRavenSmsManager _manager;

    public ClientsAddPageModel(
        IRavenSmsManager ravenSmsManager,
        IStringLocalizer<MessagesAddPageModel> localizer,
        ILogger<MessagesAddPageModel> logger)
        : base(localizer, logger)
    {
        _manager = ravenSmsManager;
    }
}
