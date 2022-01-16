﻿namespace SMS.Net.Channel.RavenSMS.EntityFramework;

/// <summary>
/// the entity configuration for <see cref="RavenSmsClient"/> entity
/// </summary>
public class RavenSmsClientEntityConfiguration : IEntityTypeConfiguration<RavenSmsClient>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<RavenSmsClient> builder)
    {
        builder.Property(e => e.PhoneNumbers)
            .HasConversion
            (
                entity => entity.ToJson(),
                json => json.FromJson<HashSet<string>>() ?? new HashSet<string>()
            );

        builder.Property(e => e.Name)
            .HasMaxLength(150);

        builder.Property(e => e.Description)
            .HasMaxLength(300);
    }
}