namespace SMS.Net.Channel.RavenSMS.Pages;

/// <summary>
/// the Messages Preview pages
/// </summary>
public partial class MessagesPreviewPageModel
{
    /// <summary>
    /// Get or set the message.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// the message data
    /// </summary>
    public RavenSmsMessage? SmsMessage { get; set; }

    /// <summary>
    /// the client used to send the message
    /// </summary>
    public RavenSmsClient? Client { get; set; }
}

/// <summary>
/// partial part for <see cref="MessagesPreviewPageModel"/>
/// </summary>
public partial class MessagesPreviewPageModel
{
    public async Task<IActionResult> OnGetAsync(string? id)
    {
        if (string.IsNullOrEmpty(id))
            return RedirectToPage("/Clients/index", new { area = "RavenSMS" });

        var message = await _messagesManager.FindByIdAsync(id);
        if (message is null)
        {
            Message = $"Couldn't find a client with the Id: {id}";
            return Page();
        }

        SmsMessage = message;
        Client = message.Client;

        return Page();
    }

    public async Task<JsonResult> OnGetResendAsync([FromRoute] string? id)
    {
        if (string.IsNullOrEmpty(id))
            return new JsonResult(new { success = false, message = "the message id is null" });

        var message = await _messagesManager.FindByIdAsync(id);
        if (message is null)
            return new JsonResult(new { success = false, message = "the message not found" });

        var queueResult = await _manager.QueueMessageAsync(message);
        if (queueResult.IsSuccess())
            return new JsonResult(new { success = true });

        return new JsonResult(new { success = false, message = queueResult.Message });
    }
}

/// <summary>
/// partial part for <see cref="MessagesPreviewPageModel"/>
/// </summary>
public partial class MessagesPreviewPageModel : BasePageModel
{
    private readonly IRavenSmsManager _manager;
    private readonly IRavenSmsMessagesManager _messagesManager;

    public MessagesPreviewPageModel(
        IRavenSmsManager manager,
        IRavenSmsMessagesManager ravenSmsManager,
        IStringLocalizer<MessagesAddPageModel> localizer,
        ILogger<MessagesAddPageModel> logger)
        : base(localizer, logger)
    {
        _manager = manager;
        _messagesManager = ravenSmsManager;
    }
}
