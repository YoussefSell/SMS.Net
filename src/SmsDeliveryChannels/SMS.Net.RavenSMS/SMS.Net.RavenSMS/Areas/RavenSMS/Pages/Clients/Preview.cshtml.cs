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
    /// the client qr code test
    /// </summary>
    public string? QrCodeText { get; set; } = default!;

    /// <summary>
    /// Get or set the client to setup.
    /// </summary>
    public ClientsUpdatePageModelInput Input { get; set; }

    /// <summary>
    /// the page model input
    /// </summary>
    public class ClientsUpdatePageModelInput
    {
        /// <summary>
        /// the id of the clients
        /// </summary>
        public string ClientId { get; set; } = default!;

        /// <summary>
        /// Get or set for the client.
        /// </summary>
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Get or set a description for the client.
        /// </summary>
        [MaxLength(300)]
        public string? Description { get; set; } = default!;

        /// <summary>
        /// the phone numbers associated with this client
        /// </summary>
        [Required]
        [RegularExpression(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,8}$")]
        public string PhoneNumber { get; set; } = default!;
    }
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

        Input = new ClientsUpdatePageModelInput
        {
            Name = client.Name,
            ClientId = client.Id,
            Description = client.Description,
            PhoneNumber = client.PhoneNumber,
        };

        QrCodeText = BuildClientQrCodeContent(client);

        return Page();
    }
}

/// <summary>
/// partial part for <see cref="ClientsPreviewPage"/>
/// </summary>
public partial class ClientsPreviewPage : BasePageModel
{
    private readonly IRavenSmsClientsManager _manager;

    public ClientsPreviewPage(
        IRavenSmsClientsManager ravenSmsManager,
        IStringLocalizer<ClientsPreviewPage> localizer,
        ILogger<ClientsPreviewPage> logger)
        : base(localizer, logger)
    {
        _manager = ravenSmsManager;
        Input = new ClientsUpdatePageModelInput();
    }
}
