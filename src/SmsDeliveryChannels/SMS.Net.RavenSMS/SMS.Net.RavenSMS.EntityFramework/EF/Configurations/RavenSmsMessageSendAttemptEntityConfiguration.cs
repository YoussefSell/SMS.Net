﻿using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SMS.Net.Channel.RavenSMS.EntityFramework;

/// <summary>
/// the entity configuration for <see cref="RavenSmsMessage"/> entity
/// </summary>
public class RavenSmsMessageSendAttemptEntityConfiguration : IEntityTypeConfiguration<RavenSmsMessageSendAttempt>
{
    readonly ValueComparer valueComparer = new ValueComparer<ICollection<ResultError>>(
        (c1, c2) => c1.SequenceEqual(c2),
        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
        c => c.ToList());

    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<RavenSmsMessageSendAttempt> builder)
    {
        builder.Property(e => e.Id)
            .HasMaxLength(17);

        builder.Property(e => e.Status)
            .HasConversion(new EnumToStringConverter<SendAttemptStatus>())
            .HasMaxLength(20);

        builder.Property(e => e.Errors)
            .HasConversion
            (
                entity => entity.ToJson(),
                value => value.FromJson<List<ResultError>>() ?? new List<ResultError>()
            )
            .HasMaxLength(4000)
            .Metadata.SetValueComparer(valueComparer);

        builder.HasOne(e => e.Message)
            .WithMany(e => e.SendAttempts)
            .HasForeignKey(e => e.MessageId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}