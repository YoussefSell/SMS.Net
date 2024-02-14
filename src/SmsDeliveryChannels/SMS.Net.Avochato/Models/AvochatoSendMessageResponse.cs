namespace SMS.Net.Channel.Avochato;

/// <summary>
/// the Avochato send message response
/// </summary>
public partial class AvochatoSendMessageResponse
{
    [JsonProperty("uuid")]
    public Guid? Uuid { get; set; }

    [JsonProperty("account_id")]
    public string? AccountId { get; set; }

    [JsonProperty("ticket_id")]
    public string? TicketId { get; set; }

    [JsonProperty("contact_id")]
    public string? ContactId { get; set; }

    [JsonProperty("sender_id")]
    public string? SenderId { get; set; }

    [JsonProperty("sender_type")]
    public string? SenderType { get; set; }

    [JsonProperty("to")]
    public string? To { get; set; }

    [JsonProperty("from")]
    public object? From { get; set; }

    [JsonProperty("direction")]
    public string? Direction { get; set; }

    [JsonProperty("origin")]
    public string? Origin { get; set; }

    [JsonProperty("message")]
    public string? Message { get; set; }

    [JsonProperty("contents")]
    public List<object>? Contents { get; set; }

    [JsonProperty("segments")]
    public long? Segments { get; set; }

    [JsonProperty("carrier")]
    public string? Carrier { get; set; }

    [JsonProperty("external_id")]
    public object? ExternalId { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("error_code")]
    public long? ErrorCode { get; set; }

    [JsonProperty("reason")]
    public object? Reason { get; set; }

    [JsonProperty("status_callback")]
    public Uri? StatusCallback { get; set; }

    [JsonProperty("created_at")]
    public double? CreatedAt { get; set; }

    [JsonProperty("sent_at")]
    public double? SentAt { get; set; }

    [JsonProperty("event_id")]
    public string? EventId { get; set; }

    [JsonProperty("element_id")]
    public string? ElementId { get; set; }

    [JsonProperty("element_type")]
    public string? ElementType { get; set; }
}