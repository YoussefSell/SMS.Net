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
                phoneNumbers = client.PhoneNumbers.Select(number => number.PhoneNumber).ToArray(),
            }),
        });
    }
}

/// <summary>
/// partial part for <see cref="MessagesIndexPageModel"/>
/// </summary>
public partial class ClientIndexPageModel : BasePageModel
{
    private readonly IRavenSmsManager _manager;

    public ClientIndexPageModel(
        IRavenSmsManager ravenSmsManager,
        IStringLocalizer<ClientIndexPageModel> localizer,
        ILogger<ClientIndexPageModel> logger)
        : base(localizer, logger)
    {
        _manager = ravenSmsManager;
    }
}
