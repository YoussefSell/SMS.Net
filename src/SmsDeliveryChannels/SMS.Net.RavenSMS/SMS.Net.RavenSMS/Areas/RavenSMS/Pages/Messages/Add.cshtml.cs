namespace SMS.Net.RavenSMS.Pages;

/// <summary>
/// the Messages add page
/// </summary>
public partial class MessagesAddPageModel : BasePageModel
{
    /// <summary>
    /// the input model
    /// </summary>
    [BindProperty]
    public MessagesAddPageModelInput Input { get; set; } = default!;

    /// <summary>
    /// the page model input
    /// </summary>
    public class MessagesAddPageModelInput
    {

    }
}

/// <summary>
/// partial part for <see cref="MessagesAddPageModel"/>
/// </summary>
public partial class MessagesAddPageModel
{

}


/// <summary>
/// partial part for <see cref="MessagesAddPageModel"/>
/// </summary>
public partial class MessagesAddPageModel
{
    private readonly IRavenSmsManager _manager;

    public MessagesAddPageModel(
        IRavenSmsManager ravenSmsManager,
        IStringLocalizer<MessagesAddPageModel> localizer, 
        ILogger<MessagesAddPageModel> logger) 
        : base(localizer, logger)
    {
        _manager = ravenSmsManager;
    }
}
