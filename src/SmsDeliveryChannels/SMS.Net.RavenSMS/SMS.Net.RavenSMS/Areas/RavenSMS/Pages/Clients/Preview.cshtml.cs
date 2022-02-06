namespace SMS.Net.RavenSMS.Pages;

/// <summary>
/// the Messages index pages
/// </summary>
public partial class ClientsPreviewPage
{
    /// <summary>
    /// Get or set the message.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Get or set the client to setup.
    /// </summary>
    public RavenSmsClient? Client { get; set; }
}

/// <summary>
/// partial part for <see cref="ClientsPreviewPage"/>
/// </summary>
public partial class ClientsPreviewPage
{
    public async Task<IActionResult> OnGetAsync(string? id)
    {
        if (string.IsNullOrEmpty(id))
            return RedirectToPage("/Clients/index", new { area = "RavenSMS" });

        var client = await _manager.FindClientByIdAsync(id);
        if (client is null)
        {
            Message = $"Couldn't find a client with the Id: {id}";
            return Page();
        }

        Client = client;

        return Page();
    }
}

/// <summary>
/// partial part for <see cref="ClientsPreviewPage"/>
/// </summary>
public partial class ClientsPreviewPage : BasePageModel
{
    private readonly IRavenSmsManager _manager;

    public ClientsPreviewPage(
        IRavenSmsManager ravenSmsManager,
        IStringLocalizer<ClientsPreviewPage> localizer,
        ILogger<ClientsPreviewPage> logger)
        : base(localizer, logger)
    {
        _manager = ravenSmsManager;
    }
}
