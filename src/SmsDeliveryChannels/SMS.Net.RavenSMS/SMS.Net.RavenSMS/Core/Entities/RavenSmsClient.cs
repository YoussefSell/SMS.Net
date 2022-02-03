namespace SMS.Net.Channel.RavenSMS.Entities;

/// <summary>
/// a class that defines a client that is used for sending SMS messages.
/// </summary>
public class RavenSmsClient
{
    /// <summary>
    /// create and instance of <see cref="RavenSmsClient"/>
    /// </summary>
    public RavenSmsClient()
    {
        CreatedOn = DateTimeOffset.Now;
        Id = Generator.GenerateUniqueId("clt");
        Status = RavenSmsClientStatus.RequireSetup;
        PhoneNumbers = new HashSet<RavenSmsClientPhoneNumber>();
    }

    /// <summary>
    /// Get or set the id of the client.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Get or set the date
    /// </summary>
    public DateTimeOffset CreatedOn { get; set; }

    /// <summary>
    /// Get or set for the client.
    /// </summary>
    public string Name { get; set; } = default!;
    
    /// <summary>
    /// the current status of the client app.
    /// </summary>
    public RavenSmsClientStatus Status { get; set; }

    /// <summary>
    /// Get or set a description for the client.
    /// </summary>
    public string Description { get; set; } = default!;

    /// <summary>
    /// the phone numbers associated with this client
    /// </summary>
    public IEnumerable<RavenSmsClientPhoneNumber> PhoneNumbers { get; set; } 
}
