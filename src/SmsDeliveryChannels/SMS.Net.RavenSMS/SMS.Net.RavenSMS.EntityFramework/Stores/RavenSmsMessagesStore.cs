namespace SMS.Net.Channel.RavenSMS.EntityFramework;

/// <summary>
/// the store implementation for <see cref="IRavenSmsMessagesStore"/>
/// </summary>
public partial class RavenSmsMessagesStore : IRavenSmsMessagesStore
{
    /// <inheritdoc/>
    public Task<RavenSmsMessage?> FindByIdAsync(Guid messageId)
        => _context.RavenSmsMessages.FirstOrDefaultAsync(message => message.Id == messageId);

    /// <inheritdoc/>
    public Task<RavenSmsMessage[]> GetAllAsync()
        => _context.RavenSmsMessages.ToArrayAsync();

    /// <inheritdoc/>
    public async Task<(RavenSmsMessage[] data, int rowsCount)> GetAllAsync(RavenSmsMessageFilter filter)
    {
        var query = _context.RavenSmsMessages.AsQueryable();

        // apply the filter & the orderBy
        query = SetFilter(query, filter);
        query = query.DynamicOrderBy(filter.OrderBy, filter.SortDirection);

        var rowsCount = 0;

        if (!filter.IgnorePagination)
        {
            rowsCount = await query.Select(e => e.Id)
                .Distinct()
                .CountAsync();

            query = query.Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize);
        }

        var data = await query.ToArrayAsync();

        rowsCount = filter.IgnorePagination
            ? data.Length
            : rowsCount;

        return (data, rowsCount);
    }

    /// <inheritdoc/>
    public async Task<(RavenSmsMessage[] messages, int rowsCount)> GetAllMessagesAsync()
    {
        var messages = await _context.RavenSmsMessages.ToArrayAsync();
        var count = await _context.RavenSmsMessages.CountAsync();

        return (messages, count);
    }

    /// <inheritdoc/>
    public async Task<Result<RavenSmsMessage>> SaveAsync(RavenSmsMessage message)
    {
        try
        {
            var entity = _context.RavenSmsMessages.Add(message);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }
        catch (Exception ex)
        {
            return Result.Failure<RavenSmsMessage>()
                .WithMessage("Failed to save the message, an exception has been accrued")
                .WithErrors(ex);
        }
    }

    /// <inheritdoc/>
    public async Task<Result<RavenSmsMessage>> UpdateAsync(RavenSmsMessage message)
    {
        try
        {
            var entity = _context.RavenSmsMessages.Update(message);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }
        catch (Exception ex)
        {
            return Result.Failure<RavenSmsMessage>()
                .WithMessage("Failed to update the message, an exception has been accrued")
                .WithErrors(ex);
        }
    }
}

/// <summary>
/// partial part for <see cref="RavenSmsClientsStore"/>
/// </summary>
public partial class RavenSmsMessagesStore
{
    private readonly IRavenSmsDbContext _context;

    public RavenSmsMessagesStore(IRavenSmsDbContext context)
    {
        _context = context;
    }

    private static IQueryable<RavenSmsMessage> SetFilter(IQueryable<RavenSmsMessage> query, RavenSmsMessageFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.SearchQuery))
            query = query.Where(e => EF.Functions.Like(e.Body, $"%{filter.SearchQuery}%"));

        if (filter.StartDate.HasValue)
            query = query.Where(e => e.CreateOn >= filter.StartDate);

        if (filter.EndDate.HasValue)
            query = query.Where(e => e.CreateOn >= filter.EndDate);

        if (filter.Priority.HasValue)
            query = query.Where(e => e.Priority == filter.Priority);

        if (filter.Status is not null && filter.Status.Any())
            query = query.Where(e => filter.Status.Contains(e.Status));

        if (filter.To is not null && filter.To.Any())
            query = query.Where(e => filter.To.Contains((string)e.To));

        if (filter.From is not null && filter.From.Any())
            query = query.Where(e => filter.From.Contains((string)e.From));

        if (filter.Clients is not null && filter.Clients.Any())
            query = query.Where(e => filter.Clients.Contains(e.ClientId));

        return query;
    }
}
