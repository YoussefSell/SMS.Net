namespace SMS.Net.Channel.RavenSMS.EntityFramework;

/// <summary>
/// the entity configuration for <see cref="RavenSmsClient"/> entity
/// </summary>
public class RavenSmsClientEntityConfiguration : IEntityTypeConfiguration<RavenSmsClient>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<RavenSmsClient> builder)
    {
        builder.Property(e => e.Id)
            .HasMaxLength(17);

        builder.Property(e => e.Name)
            .HasMaxLength(150);

        builder.Property(e => e.Description)
            .HasMaxLength(300);

        builder.HasMany(e => e.PhoneNumbers)
            .WithOne()
            .HasForeignKey(e => e.ClientId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
