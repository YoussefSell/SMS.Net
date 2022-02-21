namespace SMS.Net.RavenSMS.Pages;

/// <summary>
/// the Messages index pages
/// </summary>
public partial class ClientSetupPageModel
{
    /// <summary>
    /// Get or set the message.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Get or set the client to setup.
    /// </summary>
    public RavenSmsClient? Client { get; set; }

    /// <summary>
    /// the Qr code content
    /// </summary>
    public string? QrCodeText { get; set; }
}

/// <summary>
/// partial part for <see cref="ClientSetupPageModel"/>
/// </summary>
public partial class ClientSetupPageModel
{
    public async Task<IActionResult> OnGetAsync(string? id)
    {
        if (string.IsNullOrEmpty(id))
            return RedirectToPage("/Clients/index", new { area = "RavenSMS" });

        Client = await _manager.FindClientByIdAsync(id);
        if (Client is null)
        {
            Message = $"Couldn't find a client with the Id: {id}";
            return Page();
        }

        // build the json model
        var jsonModel = System.Text.Json.JsonSerializer.Serialize(new
        {
            clientId = Client.Id,
            clientName = Client.Name,
            clientDescription = Client.Description,
            serverUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}",
            type = "_connection_model",
        });

        // convert the json model to a base64 string
        QrCodeText = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(jsonModel));

        return Page();
    }
}

/// <summary>
/// partial part for <see cref="ClientSetupPageModel"/>
/// </summary>
public partial class ClientSetupPageModel : BasePageModel
{
    private readonly IRavenSmsClientsManager _manager;

    public ClientSetupPageModel(
        IRavenSmsClientsManager ravenSmsManager,
        IStringLocalizer<ClientSetupPageModel> localizer,
        ILogger<ClientSetupPageModel> logger)
        : base(localizer, logger)
    {
        _manager = ravenSmsManager;
    }
}

