namespace SMS.Net.Channel.D7Networks;

/// <summary>
/// the D7Networks message
/// </summary>
public partial class D7NetworksMessage
{
    [JsonProperty("messages")]
    public Message[] Messages { get; set; } = default!;

    [JsonProperty("message_globals")]
    public MessageGlobals MessageGlobals { get; set; } = default!;
}

public partial class MessageGlobals
{
    [JsonProperty("channel")]
    public string? Channel { get; set; } = "sms";

    [JsonProperty("data_coding")]
    public DataCoding? DataCoding { get; set; }

    [JsonProperty("originator")]
    public string? Originator { get; set; }

    [JsonProperty("report_url")]
    public Uri? ReportUrl { get; set; }

    [JsonProperty("schedule_time")]
    public string? ScheduleTime { get; set; }
}

public partial class Message
{
    [JsonProperty("channel")]
    public string Channel { get; set; } = "sms";

    [JsonProperty("recipients")]
    public string[] Recipients { get; set; } = default!;

    [JsonProperty("content")]
    public string Content { get; set; } = default!;

    [JsonProperty("msg_type")]
    public MessageType MessageType { get; set; } = MessageType.TEXT;

    [JsonProperty("data_coding")]
    public DataCoding DataCoding { get; set; } = DataCoding.TEXT;
}

[JsonConverter(typeof(StringEnumConverter))]
public enum MessageType
{
    [EnumMember(Value = "text")]
    TEXT,

    [EnumMember(Value = "image")]
    IMAGE,

    [EnumMember(Value = "audio sms")]
    AUDIO_SMS,

    [EnumMember(Value = "multimedia")]
    MULTIMEDIA, 
}

[JsonConverter(typeof(StringEnumConverter))]
public enum DataCoding
{
    [EnumMember(Value = "auto")]
    AUTO,

    [EnumMember(Value = "text")]
    TEXT,

    [EnumMember(Value = "unicode")]
    UNICODE,
}