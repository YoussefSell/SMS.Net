namespace SMS.Net.Channel.D7Networks;

/// <summary>
/// the D7Networks send message response
/// </summary>
public partial class D7NetworksSendMessageResponse
{
    [JsonProperty("request_id")]
    public string? RequestId { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonProperty("detail")]
    public Detail? Detail { get; set; }
}

public partial class Detail
{
    [JsonProperty("code")]
    public string? Code { get; set; }

    [JsonProperty("message")]
    public string? Message { get; set; }

    [JsonProperty("loc")]
    public string[]? Loc { get; set; }

    [JsonProperty("msg")]
    public string? Msg { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }
}