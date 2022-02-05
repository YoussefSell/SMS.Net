namespace SMS.Net.RavenSMS.Pages;

/// <summary>
/// the Messages index pages
/// </summary>
public partial class ClientSetupPageModel
{
    
}

/// <summary>
/// partial part for <see cref="ClientSetupPageModel"/>
/// </summary>
public partial class ClientSetupPageModel : BasePageModel
{
    private readonly IRavenSmsManager _manager;

    public ClientSetupPageModel(
        IRavenSmsManager ravenSmsManager,
        IStringLocalizer<ClientSetupPageModel> localizer,
        ILogger<ClientSetupPageModel> logger)
        : base(localizer, logger)
    {
        _manager = ravenSmsManager;
    }
}

