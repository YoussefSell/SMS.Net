namespace SMS.Net.Channel.RavenSMS.Pages;

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
            pagination = new
            {
                rowsCount,
                pageSize = filter.PageSize,
                pageIndex = filter.PageIndex,
            },
            data = messages.Select(message => new
            {
                message.Id,
                to = message.To.ToString(),
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
    private readonly IRavenSmsMessagesManager _manager;

    public MessagesIndexPageModel(
        IRavenSmsMessagesManager ravenSmsManager,
        IStringLocalizer<MessagesAddPageModel> localizer,
        ILogger<MessagesAddPageModel> logger)
        : base(localizer, logger)
    {
        _manager = ravenSmsManager;
    }
}