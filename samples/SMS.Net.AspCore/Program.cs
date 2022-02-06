var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

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

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapRavenSmsHub();

app.Run();
