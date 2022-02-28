﻿namespace SMS.Net.RavenSMS.Pages;

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
}

/// <summary>
/// partial part for <see cref="MessagesIndexPageModel"/>
/// </summary>
public partial class ClientIndexPageModel : BasePageModel
{
    private readonly IRavenSmsClientsManager _manager;

    public ClientIndexPageModel(
        IRavenSmsClientsManager ravenSmsManager,
        IStringLocalizer<ClientIndexPageModel> localizer,
        ILogger<ClientIndexPageModel> logger)
        : base(localizer, logger)
    {
        _manager = ravenSmsManager;
    }
}
