namespace SMS.Net.RavenSMS.Pages;

/// <summary>
/// the base page model
/// </summary>
public class BasePageModel : PageModel
{
    protected readonly IStringLocalizer _localizer;
    protected readonly ILogger _logger;

    public BasePageModel(IStringLocalizer localizer, ILogger logger)
    {
        _logger = logger;
        _localizer = localizer;
    }

    /// <summary>
    /// the status message a TempData used to share alert messages between pages
    /// </summary>
    [TempData]
    public string? StatusMessage { get; set; }

    /// <summary>
    /// translate the given name string, using the provided translation implantation
    /// </summary>
    /// <param name="name">the name to localize</param>
    /// <param name="arguments">the args if any</param>
    /// <returns>the translated text</returns>
    public virtual string Localize(string name, params object[] arguments)
    {
        var translation = _localizer[name, arguments];
        if (translation is not null && !translation.ResourceNotFound)
            return translation.Value;

        return name;
    }

    protected string BuildClientQrCodeContent(RavenSmsClient client)
    {
        // build the json model
        var jsonModel = System.Text.Json.JsonSerializer.Serialize(new
        {
            clientId = client.Id,
            serverUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}",
        });

        // convert the json model to a base64 string
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(jsonModel));
    }
}
