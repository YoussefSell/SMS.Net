var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSignalR();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddCors(options =>
{
    options.AddPolicy("ionic-cors",
        builder => builder
        //.WithOrigins(
        //    "http://localhost",
        //    "ionic://localhost",
        //    "capacitor://localhost",
        //    "http://localhost:8100", 
        //    "http://192.168.1.99:8100",
        //    "http://192.168.1.102:8100"
        //)
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

// Add Hangfire services.
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseMariaDb(builder.Configuration.GetConnectionString("hangfire")));

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();

// add SMS.Net services
builder.Services.AddSMSNet(options =>
{
    options.PauseSending = false;
    options.DefaultFrom = new SMS.Net.PhoneNumber("+21206060606");
    options.DefaultDeliveryChannel = RavenSmsDeliveryChannel.Name;
})
.UseAvochato(authId: "Key-1", authSecret: "Key-1")
.UseTwilio(username: "Key-1", password: "Key-1")
.UseMessageBird(accessKey: "Key-1")
.UseRavenSMS(options =>
{
    options.UseHangfireQueue();
    options.UseEntityFrameworkStores<ApplicationDbContext>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use((context, next) =>
{
    return next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapHangfireDashboard();

app.MapRavenSmsHub();

app.Run();
