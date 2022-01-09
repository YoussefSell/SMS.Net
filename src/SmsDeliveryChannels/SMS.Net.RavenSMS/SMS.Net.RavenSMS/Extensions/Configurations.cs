namespace Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

/// <summary>
/// the Configurations class
/// </summary>
public static class Configurations
{
    /// <summary>
    /// add the Twilio channel to be used with your SMS service.
    /// </summary>
    /// <param name="builder">the SMSNet builder instance.</param>
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
        builder.ServiceCollection.AddScoped<ISmsChannel, RavenSmsDeliveryChannel>();
        builder.ServiceCollection.AddScoped<IRavenSmsDeliveryChannel, RavenSmsDeliveryChannel>();

        builder.ServiceCollection.AddScoped<IRavenSmsClientsManager, RavenSmsClientsManager>();
        builder.ServiceCollection.AddScoped<IRavenSmsManager, RavenSmsManager>();

        builder.ServiceCollection.ConfigureOptions(typeof(RavenSmsUIConfigureOptions));

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

