namespace SMS.Net.RavenSMS.Stores.Defaults;

/// <summary>
/// the default implementation for <see cref="IRavenSmsMessagesStore"/> with an in memory store
/// </summary>
public partial class RavenSmsMessagesInMemoryStore
{
    /// <inheritdoc/>
    public async Task<(long totalSent, long totalFailed, long totalInQueue)> GetCountsAsync(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        var result = _messages.GroupBy(e => e.Status)
            .Select(grouping => new
            {
                grouping.Key,
                Count = grouping.Count()
            })
            .ToDictionary(e => e.Key, e => e.Count);

        return (
            result.TryGetValue(RavenSmsMessageStatus.Sent, out var totalSent) ? totalSent : 0,
            result.TryGetValue(RavenSmsMessageStatus.Failed, out var totalFailed) ? totalFailed : 0,
            result.TryGetValue(RavenSmsMessageStatus.Queued, out var totalQueued) ? totalQueued : 0
        );
    }

    /// <inheritdoc/>
    public Task<RavenSmsMessage[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(_messages.ToArray());
    }

    /// <inheritdoc/>
    public async Task<(RavenSmsMessage[] data, int rowsCount)> GetAllAsync(RavenSmsMessageFilter filter, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        // apply the filter & the orderBy
        var query = SetFilter(_messages, filter);

        var rowsCount = 0;

        if (!filter.IgnorePagination)
        {
            rowsCount = query.Select(e => e.Id)
                .Distinct()
                .Count();

            query = query.Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize);
        }

        var data = query.ToArray();

        rowsCount = filter.IgnorePagination
            ? data.Length
            : rowsCount;

        return (data, rowsCount);
    }

    /// <inheritdoc/>
    public Task<bool> AnyAsync(string messageId, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(_messages.Any(message => message.Id == messageId));
    }

    /// <inheritdoc/>
    public Task<RavenSmsMessage?> FindByIdAsync(string messageId, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(_messages.FirstOrDefault(message => message.Id == messageId));
    }

    /// <inheritdoc/>
    public Task<Result<RavenSmsMessage>> CreateAsync(RavenSmsMessage message, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        _messages.Add(message);

        return Task.FromResult(Result.Success(message));
    }

    /// <inheritdoc/>
    public Task<Result<RavenSmsMessage>> UpdateAsync(RavenSmsMessage message, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        var messageToUpdate = _messages.FirstOrDefault(c => c.Id == message.Id);
        if (messageToUpdate == null)
        {
            return Task.FromResult(Result.Failure<RavenSmsMessage>()
                .WithMessage("Failed to update the message, not found")
                .WithCode("message_not_found"));
        }

        messageToUpdate.To = message.To;
        messageToUpdate.Body = message.Body;
        messageToUpdate.Status = message.Status;
        messageToUpdate.SentOn = message.SentOn;
        messageToUpdate.Priority = message.Priority;
        messageToUpdate.ClientId = message.ClientId;
        messageToUpdate.CreateOn = message.CreateOn;
        messageToUpdate.DeliverAt = message.DeliverAt;
        messageToUpdate.SendAttempts = message.SendAttempts;

        return Task.FromResult(Result.Success(messageToUpdate));
    }
}

/// <summary>
/// partial part for <see cref="RavenSmsMessagesInMemoryStore"/>
/// </summary>
public partial class RavenSmsMessagesInMemoryStore
{
    const string _dateFormat = "yyyy-MM-ddTHH:mm:sszzz";
    private readonly ICollection<RavenSmsMessage> _messages;

    public RavenSmsMessagesInMemoryStore()
    {
        _messages = new List<RavenSmsMessage>();
    }

    private static IEnumerable<RavenSmsMessage> SetFilter(IEnumerable<RavenSmsMessage> query, RavenSmsMessageFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.SearchQuery))
            query = query.Where(e => e.Body.Contains(filter.SearchQuery));

        if (!string.IsNullOrEmpty(filter.StartDate) && DateTimeOffset.TryParseExact(filter.StartDate, _dateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var startDate))
            query = query.Where(e => e.CreateOn.Date >= startDate.Date);

        if (!string.IsNullOrEmpty(filter.EndDate) && DateTimeOffset.TryParseExact(filter.EndDate, _dateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var endDate))
            query = query.Where(e => e.CreateOn.Date <= endDate.Date);

        if (filter.Priority.HasValue)
            query = query.Where(e => e.Priority == filter.Priority);

        if (filter.Status != RavenSmsMessageStatus.None)
            query = query.Where(e => filter.Status == e.Status);

        if (filter.To is not null && filter.To.Any())
            query = query.Where(e => filter.To.Contains((string)e.To));

        if (filter.Clients is not null && filter.Clients.Any())
            query = query.Where(e => filter.Clients.Contains(e.ClientId));

        return query;
    }
}
