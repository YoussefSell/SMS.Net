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
    public ClientsAddPageModelInput Input { get; set; }

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
        public string? PhoneNumbers { get; set; }

        /// <summary>
        /// get the list of phone numbers from the <see cref="PhoneNumbers"/> property
        /// </summary>
        /// <returns>a list of phone numbers</returns>
        public IEnumerable<string> GetPhoneNumbers()
            => (PhoneNumbers ?? string.Empty).Split(',').Where(e => !string.IsNullOrEmpty(e));
    }
}

/// <summary>
/// partial part for <see cref="ClientsAddPageModel"/>
/// </summary>
public partial class ClientsAddPageModel : BasePageModel
{
    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            // create message instance
            var client = new RavenSmsClient
            {
                Name = Input.Name,
                Description = Input.Description,
                PhoneNumbers = Input.GetPhoneNumbers()
                    .Select(number => new RavenSmsClientPhoneNumber() { PhoneNumber = number })
                    .ToArray(),
            };

            // add the client
            var result = await _manager.CreateClientAsync(client);
            if (result.IsSuccess())
            {
                // client added successfully
                return RedirectToPage("/Clients/index", new { area = "RavenSMS" });
            }

            ModelState.AddModelError("", result.Message);
        }

        return Page();
    }

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
        Input = new ClientsAddPageModelInput();
    }
}
