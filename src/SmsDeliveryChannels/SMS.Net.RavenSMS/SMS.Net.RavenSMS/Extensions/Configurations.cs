namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// the Configurations class
/// </summary>
public static class Configurations
{
    /// <summary>
    /// add the RavenSMS channel to be used with your SMS service.
    /// </summary>
    /// <param name="builder">the SMS.Net builder instance.</param>
    /// <param name="config">the configuration builder instance.</param>
    /// <returns>instance of <see cref="SmsNetBuilder"/> to enable methods chaining.</returns>
    public static SmsNetBuilder UseRavenSMS(this SmsNetBuilder builder, Action<RavenSmsBuilder> config)
    {
        // load the configuration
        var builderOptions = new RavenSmsBuilder(builder);
        config(builderOptions);

        // build and validate the options
        var options = builderOptions.InitOptions();
        options.Validate();

        builder.ServiceCollection.AddSingleton((s) => options);
        builder.ServiceCollection.ConfigureOptions(typeof(RavenSmsUIConfigureOptions));
        
        builder.ServiceCollection.AddScoped<IRavenSmsManager, RavenSmsManager>();
        builder.ServiceCollection.AddScoped<IRavenSmsClientsManager, RavenSmsClientsManager>();
        builder.ServiceCollection.AddScoped<IRavenSmsMessagesManager, RavenSmsMessagesManager>();

        builder.ServiceCollection.AddScoped<ISmsDeliveryChannel, RavenSmsDeliveryChannel>();
        builder.ServiceCollection.AddScoped<IRavenSmsDeliveryChannel, RavenSmsDeliveryChannel>();

        return builder;
    }

    /// <summary>
    /// Maps incoming requests with the ravenSMS hub path to the <see cref="RavenSmsHub"/>
    /// </summary>
    /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
    public static void MapRavenSmsHub(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHub<RavenSmsHub>("RavenSMS/Hub");
    }

    /// <summary>
    /// Set RavenSMS to use in memory stores to persist the data.
    /// </summary>
    /// <param name="builder">the RavenSMS builder instance.</param>
    /// <returns>instance of <see cref="RavenSmsBuilder"/> to enable methods chaining.</returns>
    public static RavenSmsBuilder UseInMemoryStores(this RavenSmsBuilder builder)
    {
        builder.ServiceCollection.AddSingleton<IRavenSmsClientsStore, RavenSmsClientsInMemoryStore>();
        builder.ServiceCollection.AddSingleton<IRavenSmsMessagesStore, RavenSmsMessagesInMemoryStore>();

        return builder;
    }

    /// <summary>
    /// Set RavenSMS to use in memory Queue to queue and process sending events.
    /// </summary>
    /// <param name="builder">the RavenSMS builder instance.</param>
    /// <returns>instance of <see cref="RavenSmsBuilder"/> to enable methods chaining.</returns>
    public static RavenSmsBuilder UseInMemoryQueue(this RavenSmsBuilder builder)
    {
        builder.ServiceCollection.AddSingleton<IQueueManager, InMemoryQueueManager>();
        builder.ServiceCollection.AddSingleton<IInMemoryQueue, InMemoryQueue>();
        builder.ServiceCollection.AddHostedService<InMemoryQueueHost>();

        return builder;
    }
}

internal class RavenSmsUIConfigureOptions : IPostConfigureOptions<StaticFileOptions>
{
    private readonly IWebHostEnvironment _environment;

    public RavenSmsUIConfigureOptions(IWebHostEnvironment environment) 
        => _environment = environment;

    public void PostConfigure(string name, StaticFileOptions options)
    {
        // Basic initialization in case the options weren't initialized by any other component
        options.ContentTypeProvider ??= new FileExtensionContentTypeProvider();

        // make sure that at least we have an instance of the file provider
        if (options.FileProvider == null && _environment.WebRootFileProvider == null)
            throw new InvalidOperationException("Missing FileProvider.");

        // init the file provider
        options.FileProvider ??= _environment.WebRootFileProvider;

        // add the file provider to load the package assets
        options.FileProvider = new CompositeFileProvider(
            options.FileProvider, 
            new ManifestEmbeddedFileProvider(GetType().Assembly, "Assets"));
    }
}

