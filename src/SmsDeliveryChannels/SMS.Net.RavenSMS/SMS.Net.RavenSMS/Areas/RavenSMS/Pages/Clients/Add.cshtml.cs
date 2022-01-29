namespace SMS.Net.RavenSMS.Pages;

/// <summary>
/// the Clients add page
/// </summary>
public partial class ClientsAddPageModel
{
    /// <summary>
    /// the input model
    /// </summary>
    [BindProperty]
    public ClientsAddPageModelInput Input { get; set; } = default!;

    /// <summary>
    /// the page model input
    /// </summary>
    public class ClientsAddPageModelInput
    {
        /// <summary>
        /// Get or set for the client.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Get or set a description for the client.
        /// </summary>
        public string Description { get; set; } = default!;

        /// <summary>
        /// the phone numbers associated with this client
        /// </summary>
        public IEnumerable<string> PhoneNumbers { get; set; } = default!;
    }
}

/// <summary>
/// partial part for <see cref="ClientsAddPageModel"/>
/// </summary>
public partial class ClientsAddPageModel : BasePageModel
{
    public async Task<JsonResult> OnGetPhoneNumberExistAsync(string phoneNumber)
    {
        // return json result instance
        return new JsonResult(new
        {
            exist = await _manager.AnyClientAsync(phoneNumber)
        });
    }
}

/// <summary>
/// partial part for <see cref="ClientsAddPageModel"/>
/// </summary>
public partial class ClientsAddPageModel : BasePageModel
{
    private readonly IRavenSmsManager _manager;

    public ClientsAddPageModel(
        IRavenSmsManager ravenSmsManager,
        IStringLocalizer<MessagesAddPageModel> localizer,
        ILogger<MessagesAddPageModel> logger)
        : base(localizer, logger)
    {
        _manager = ravenSmsManager;
    }
}
