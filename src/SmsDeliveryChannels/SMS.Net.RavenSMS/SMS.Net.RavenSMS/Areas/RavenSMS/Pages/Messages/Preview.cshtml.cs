namespace SMS.Net.RavenSMS.Pages;

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

        var message = await _manager.FindByIdAsync(id);
        if (message is null)
        {
            Message = $"Couldn't find a client with the Id: {id}";
            return Page();
        }

        SmsMessage = message;
        Client = message.Client;

        return Page();
    }
}

/// <summary>
/// partial part for <see cref="MessagesPreviewPageModel"/>
/// </summary>
public partial class MessagesPreviewPageModel : BasePageModel
{
    private readonly IRavenSmsMessagesManager _manager;

    public MessagesPreviewPageModel(
        IRavenSmsMessagesManager ravenSmsManager,
        IStringLocalizer<MessagesAddPageModel> localizer,
        ILogger<MessagesAddPageModel> logger)
        : base(localizer, logger)
    {
        _manager = ravenSmsManager;
    }
}
