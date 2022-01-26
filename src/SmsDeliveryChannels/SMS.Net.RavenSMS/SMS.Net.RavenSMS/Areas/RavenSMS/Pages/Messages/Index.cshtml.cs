namespace SMS.Net.RavenSMS.Pages;

/// <summary>
/// the Messages index pages
/// </summary>
public partial class MessagesIndexPageModel
{
    public async Task<JsonResult> OnGetMessagesAsync([FromQuery] RavenSmsMessageFilter filter)
    {
        var (messages, rowsCount) = await _manager.GetAllMessagesAsync(filter);

        return new JsonResult(new
        {
            rowsCount,
            data = messages.Select(message => new
            {
                message.Id,
                to = message.To.ToString(),
                from = message.From.ToString(),
                message.Status,
                date = message.CreateOn,
                client = new
                {
                    message.Client?.Id,
                    message.Client?.Name,
                },
            }),
        });
    }
}

/// <summary>
/// partial part for <see cref="MessagesIndexPageModel"/>
/// </summary>
public partial class MessagesIndexPageModel : BasePageModel
{
    private readonly IRavenSmsManager _manager;

    public MessagesIndexPageModel(
        IRavenSmsManager ravenSmsManager,
        IStringLocalizer<MessagesAddPageModel> localizer,
        ILogger<MessagesAddPageModel> logger)
        : base(localizer, logger)
    {
        _manager = ravenSmsManager;
    }
}