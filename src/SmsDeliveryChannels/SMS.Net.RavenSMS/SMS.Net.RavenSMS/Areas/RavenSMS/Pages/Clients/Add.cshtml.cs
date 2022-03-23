using System.ComponentModel.DataAnnotations;

namespace SMS.Net.Channel.RavenSMS.Pages;

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
        [
            Required,
            RegularExpression(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,8}$"),
            PageRemote(
                HttpMethod = "get",
                PageHandler = "PhoneNumberExist",
                ErrorMessage = "the given phone number already used by another client app."
            )
        ]
        public string PhoneNumber { get; set; } = default!;
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
            if (await _clientsManager.AnyClientAsync(Input.PhoneNumber))
            {
                ModelState.AddModelError("", "the given phone number already used by another client app.");
                return Page();
            }

            // create message instance
            var client = new RavenSmsClient
            {
                Name = Input.Name,
                PhoneNumber = Input.PhoneNumber,
                Description = Input.Description ?? string.Empty,
            };

            // add the client
            var result = await _clientsManager.SaveClientAsync(client);
            if (result.IsSuccess())
            {
                // client added successfully
                return RedirectToPage("/Clients/index", new { area = "RavenSMS" });
            }

            ModelState.AddModelError("", result.Message);
        }

        return Page();
    }

    public async Task<JsonResult> OnGetPhoneNumberExistAsync([FromQuery(Name = "Input.PhoneNumber")] string phoneNumber)
    {
        // return json result instance
        return new JsonResult(!await _clientsManager.AnyClientAsync(phoneNumber));
    }
}

/// <summary>
/// partial part for <see cref="ClientsAddPageModel"/>
/// </summary>
public partial class ClientsAddPageModel : BasePageModel
{
    private readonly IRavenSmsClientsManager _clientsManager;

    public ClientsAddPageModel(
        IRavenSmsClientsManager clientsManager,
        IStringLocalizer<MessagesAddPageModel> localizer,
        ILogger<MessagesAddPageModel> logger)
        : base(localizer, logger)
    {
        _clientsManager = clientsManager;
        Input = new ClientsAddPageModelInput();
    }
}
