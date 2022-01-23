namespace SMS.Net.RavenSMS.Pages;

/// <summary>
/// the Messages Preview pages
/// </summary>
public partial class MessagesPreviewPageModel
{
}

/// <summary>
/// partial part for <see cref="MessagesPreviewPageModel"/>
/// </summary>
public partial class MessagesPreviewPageModel : BasePageModel
{
    private readonly IRavenSmsManager _manager;

    public MessagesPreviewPageModel(
        IRavenSmsManager ravenSmsManager,
        IStringLocalizer<MessagesAddPageModel> localizer,
        ILogger<MessagesAddPageModel> logger)
        : base(localizer, logger)
    {
        _manager = ravenSmsManager;
    }
}
