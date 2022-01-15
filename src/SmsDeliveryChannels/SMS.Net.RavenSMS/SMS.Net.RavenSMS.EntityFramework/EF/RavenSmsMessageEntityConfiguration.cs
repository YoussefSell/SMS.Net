using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SMS.Net.Channel.RavenSMS.EntityFramework;

/// <summary>
/// the entity configuration for <see cref="RavenSmsMessage"/> entity
/// </summary>
public class RavenSmsMessageEntityConfiguration : IEntityTypeConfiguration<RavenSmsMessage>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<RavenSmsMessage> builder)
    {
        builder.Property(e => e.Priority)
            .HasConversion(new EnumToStringConverter<Priority>());

        builder.Property(e => e.Status)
            .HasConversion(new EnumToStringConverter<Enums.RavenSmsMessageStatus>());

        builder.Property(e => e.Body)
            .HasMaxLength(500);

        builder.Property(e => e.JobQueueId)
            .HasMaxLength(100);

        builder.Property(e => e.To)
            .HasConversion
            (
                entity => entity.ToString(),
                value => new PhoneNumber(value)
            );

        builder.Property(e => e.From)
            .HasConversion
            (
                entity => entity.ToString(),
                value => new PhoneNumber(value)
            );
    }
}
