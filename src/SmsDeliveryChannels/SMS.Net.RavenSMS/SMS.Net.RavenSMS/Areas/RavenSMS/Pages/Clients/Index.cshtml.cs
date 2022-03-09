namespace SMS.Net.RavenSMS.Pages;

/// <summary>
/// the Messages index pages
/// </summary>
public partial class ClientIndexPageModel
{
    public async Task<JsonResult> OnGetClientsAsync([FromQuery] RavenSmsClientsFilter filter)
    {
        var (clients, rowsCount) = await _manager.GetAllClientsAsync(filter);

        return new JsonResult(new
        {
            pagination = new
            {
                rowsCount,
                pageSize = filter.PageSize,
                pageIndex = filter.PageIndex,
            },
            data = clients.Select(client => new
            {
                client.Id,
                name = client.Name,
                status = client.Status,
                phoneNumber = client.PhoneNumber,
            }),
        });
    }

    public async Task<JsonResult> OnGetRemoveClient([FromQuery(Name = "clientId")] string? id)
    {
        // if the id is null redirect to the clients list to select a client
        if (string.IsNullOrEmpty(id))
            return new JsonResult(new { isSuccess = false, error = "the given id is null or empty" });

        // get the client by the id, if not exist, redirect to the clients list to select an existed client
        var clientInDatabase = await _manager.FindClientByIdAsync(id);
        if (clientInDatabase is null)
            return new JsonResult(new { isSuccess = false, error = "there is no client with the given id" });

        // delete the client
        var deleteResult = await _manager.DeleteClientAsync(clientInDatabase.Id);
        if (deleteResult.IsSuccess())
        {
            // force the client to disconnect
            await _hubContext.ForceDisconnectAsync(clientInDatabase, "client_deleted");

            // return success result
            return new JsonResult(new { isSuccess = true });
        }

        return new JsonResult(new { isSuccess = false, error = deleteResult.Message });
    }
}

/// <summary>
/// partial part for <see cref="MessagesIndexPageModel"/>
/// </summary>
public partial class ClientIndexPageModel : BasePageModel
{
    private readonly IRavenSmsClientsManager _manager;
    private readonly IHubContext<RavenSmsHub> _hubContext;

    public ClientIndexPageModel(
        IHubContext<RavenSmsHub> hubContext,
        IRavenSmsClientsManager ravenSmsManager,
        IStringLocalizer<ClientIndexPageModel> localizer,
        ILogger<ClientIndexPageModel> logger)
        : base(localizer, logger)
    {
        _hubContext = hubContext;
        _manager = ravenSmsManager;
    }
}
