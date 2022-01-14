namespace SMS.Net.Channel.RavenSMS.EntityFramework;

/// <summary>
/// the store implementation for <see cref="IRavenSmsMessagesStore"/>
/// </summary>
public partial class RavenSmsMessagesStore : IRavenSmsMessagesStore
{
    /// <inheritdoc/>
    public Task<RavenSmsMessage?> FindByIdAsync(Guid messageId)
        => _context.Messages.FirstOrDefaultAsync(message => message.Id == messageId);

    /// <inheritdoc/>
    public async Task<Result<RavenSmsMessage>> SaveAsync(RavenSmsMessage message)
    {
        try
        {
            var entity = _context.Messages.Add(message);
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
            var entity = _context.Messages.Update(message);
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
}
