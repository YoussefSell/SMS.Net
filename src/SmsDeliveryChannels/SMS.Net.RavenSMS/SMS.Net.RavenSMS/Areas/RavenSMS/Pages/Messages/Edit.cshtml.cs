namespace SMS.Net.RavenSMS.Pages;

/// <summary>
/// the Messages edit pages
/// </summary>
public partial class MessagesEditPageModel
{
}

/// <summary>
/// partial part for <see cref="MessagesEditPageModel"/>
/// </summary>
public partial class MessagesEditPageModel : BasePageModel
{
    private readonly IRavenSmsManager _manager;

    public MessagesEditPageModel(
        IRavenSmsManager ravenSmsManager,
        IStringLocalizer<MessagesAddPageModel> localizer,
        ILogger<MessagesAddPageModel> logger)
        : base(localizer, logger)
    {
        _manager = ravenSmsManager;
    }
}

