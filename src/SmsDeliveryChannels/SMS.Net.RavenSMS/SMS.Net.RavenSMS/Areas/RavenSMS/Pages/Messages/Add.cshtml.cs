namespace SMS.Net.RavenSMS.Pages;

/// <summary>
/// the Messages add page
/// </summary>
public partial class MessagesAddPageModel : BasePageModel
{
    /// <summary>
    /// the input model
    /// </summary>
    [BindProperty]
    public MessagesAddPageModelInput Input { get; set; } = default!;

    /// <summary>
    /// the page model input
    /// </summary>
    public class MessagesAddPageModelInput
    {
        /// <summary>
        /// Gets or sets the priority of this e-mail message.
        /// </summary>
        public Priority Priority { get; set; }

        /// <summary>
        /// Get or set the message body.
        /// </summary>
        public string Body { get; set; } = default!;

        /// <summary>
        /// Get or set the phone numbers of recipients to send the SMS message to.
        /// </summary>
        public string To { get; set; } = default!;

        /// <summary>
        /// Get or set the phone number used to send the SMS message from it.
        /// </summary>
        public string From { get; set; } = default!;

        /// <summary>
        /// Get or set the id of the client used to send this message.
        /// </summary>
        public string Client { get; set; } = default!;

        /// <summary>
        /// the delivery date
        /// </summary>
        public DateTime? DeliveryDate { get; set; }
    }
}

/// <summary>
/// partial part for <see cref="MessagesAddPageModel"/>
/// </summary>
public partial class MessagesAddPageModel
{
    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            // create message instance
            var message = new RavenSmsMessage
            {
                To = Input.To,
                Body = Input.Body,
                From = Input.From,
                ClientId = Input.Client,
                Priority = Input.Priority,
                Status = RavenSmsMessageStatus.Created,
            };

            // if delivery date is specified queue without a delay
            if (Input.DeliveryDate is null)
                await _manager.QueueMessageAsync(message);

            else
            {
                // calculate the delay
                var delay = Input.DeliveryDate.Value - DateTime.UtcNow;

                // queue the message with the delay
                await _manager.QueueMessageAsync(message, delay);
            }

            return RedirectToPage("/Messages/index", new { area = "RavenSMS" });
        }

        return Page();
    }

    public async Task<JsonResult> OnGetClientsAsync()
    {
        // get the list of all clients
        var clients = await _clientsManager.GetAllClientsAsync();

        // convert the clients to models
        var clientsModels = clients.Select(client => new
        {
            client.Id,
            client.Name,
            client.PhoneNumber,
        });

        // return json result instance
        return new JsonResult(clientsModels);
    }
}

/// <summary>
/// partial part for <see cref="MessagesAddPageModel"/>
/// </summary>
public partial class MessagesAddPageModel
{
    private readonly IRavenSmsManager _manager;
    private readonly IRavenSmsClientsManager _clientsManager;

    public MessagesAddPageModel(
        IRavenSmsManager ravenSmsManager,
        IRavenSmsClientsManager clientsManager,
        IStringLocalizer<MessagesAddPageModel> localizer,
        ILogger<MessagesAddPageModel> logger)
        : base(localizer, logger)
    {
        _manager = ravenSmsManager;
        _clientsManager = clientsManager;
    }
}
