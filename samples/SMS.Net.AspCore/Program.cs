using SMS.Net.Channel.RavenSMS;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSMSNet(options =>
{
    options.PauseSending = false;
    options.DefaultFrom = new SMS.Net.PhoneNumber("+21206060606");
    options.DefaultDeliveryChannel = RavenSmsDeliveryChannel.Name;
})
.UseAvochato(authId: "", authSecret: "")
.UseTwilio(username: "", password: "")
.UseMessageBird(accessKey: "")
.UseRavenSMS(options =>
{

});

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

app.Run();
