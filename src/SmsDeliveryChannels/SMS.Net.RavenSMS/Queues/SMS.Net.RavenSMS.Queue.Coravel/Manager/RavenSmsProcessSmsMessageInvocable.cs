namespace SMS.Net.Channel.RavenSMS.Coravel;

/// <summary>
/// the Invocable for processing RavenSMS messages
/// </summary>
public class RavenSmsProcessSmsMessageInvocable : IInvocable, IInvocableWithPayload<InvocablePayload>
{
    private readonly IRavenSmsManager _manager;

    public RavenSmsProcessSmsMessageInvocable(IRavenSmsManager manager) 
        => _manager = manager;

    /// <inheritdoc/>
    public InvocablePayload Payload { get; set; } = default!;

    /// <inheritdoc/>
    public Task Invoke()
    {
        if (Payload is null)
            throw new ArgumentNullException("IInvocable.Payload", "the payload is null");

        return ProcessAsync();
    }

    private async Task ProcessAsync()
    {
        if (Payload.Delay.HasValue)
            await Task.Delay(Payload.Delay.Value);

        await _manager.ProcessAsync(Payload.MessageId);
    }
}
