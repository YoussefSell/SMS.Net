namespace SMS.Net.Channel.RavenSMS.EntityFramework;

/// <summary>
/// the entity configuration for <see cref="RavenSmsClient"/> entity
/// </summary>
public class RavenSmsClientPhoneNumberEntityConfiguration : IEntityTypeConfiguration<RavenSmsClientPhoneNumber>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<RavenSmsClientPhoneNumber> builder)
    {
        builder.HasKey(e => e.PhoneNumber);

        builder.Property(e => e.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(e => e.ClientId)
            .HasMaxLength(17);
    }
}