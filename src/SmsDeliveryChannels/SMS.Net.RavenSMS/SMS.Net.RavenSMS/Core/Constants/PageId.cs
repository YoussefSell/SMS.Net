namespace SMS.Net.RavenSMS.Pages;

/// <summary>
/// holds the pages ids
/// </summary>
internal static class PageId
{
    /// <summary>
    /// defines the root pages in the menu
    /// </summary>
    public enum Page { dashbord, clients, messages, preferences };

    public const string DashboardPage = "dashboard.index";

    public const string ClientAddPage       = "clients.add";
    public const string ClientIndexPage     = "clients.index";
    public const string ClientSetupPage     = "clients.setup";
    public const string ClientPreviewPage = "clients.view";

    public const string MessagesAddPage     = "messages.add";
    public const string MessagesIndexPage   = "messages.index";
    public const string MessagesPreviewPage = "messages.view";
    
    public const string PreferencesPage = "preferences.index";

    /// <summary>
    /// check if the given page is selected
    /// </summary>
    /// <param name="page">the root page</param>
    /// <param name="pageId">the current selected page Id</param>
    /// <returns></returns>
    public static bool IsPageSelected(Page page, string? pageId) 
        => pageId switch
        {
            DashboardPage => page == Page.dashbord,
            PreferencesPage => page == Page.preferences,
            MessagesAddPage or MessagesIndexPage or MessagesPreviewPage => page == Page.messages,
            ClientAddPage or ClientIndexPage or ClientSetupPage or ClientPreviewPage => page == Page.clients,
            _ => false,
        };
}
