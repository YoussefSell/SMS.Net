namespace SMS.Net.Channel.D7Networks;

/// <summary>
/// the options for configuring the D7Networks SMS delivery channel
/// </summary>
public class D7NetworksSmsDeliveryChannelOptions
{
    /// <summary>
    /// Get or Set your D7Networks api-key.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// the base url to the D7Networks service, default value is: 'https://api.d7networks.com'
    /// </summary>
    public string BaseUrl { get; set; } = "https://api.d7networks.com";

    /// <summary>
    /// validate if the options are all set correctly
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(ApiKey))
            throw new RequiredOptionValueNotSpecifiedException<D7NetworksSmsDeliveryChannelOptions>(
                $"{nameof(ApiKey)}", "the given D7NetworksSmsDeliveryChannelOptions.ApiKey value is null or empty.");

        if (string.IsNullOrWhiteSpace(BaseUrl))
            throw new RequiredOptionValueNotSpecifiedException<D7NetworksSmsDeliveryChannelOptions>(
                $"{nameof(BaseUrl)}", "the given D7NetworksSmsDeliveryChannelOptions.BaseUrl value is null or empty.");
    }
}
