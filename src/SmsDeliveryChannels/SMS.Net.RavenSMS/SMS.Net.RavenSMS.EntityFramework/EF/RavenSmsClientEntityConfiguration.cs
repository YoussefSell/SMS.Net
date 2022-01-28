using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SMS.Net.Channel.RavenSMS.EntityFramework;

/// <summary>
/// the entity configuration for <see cref="RavenSmsClient"/> entity
/// </summary>
public class RavenSmsClientEntityConfiguration : IEntityTypeConfiguration<RavenSmsClient>
{
    private static readonly ValueComparer<IEnumerable<string>> PhoneNumbers_ValueComparer =
        new((c1, c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => (IEnumerable<string>)c.ToHashSet());

    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<RavenSmsClient> builder)
    {
        builder.Property(e => e.Id)
            .HasMaxLength(17);

        builder.Property(e => e.PhoneNumbers)
            .HasConversion
            (
                entity => entity.ToJson(),
                json => json.FromJson<HashSet<string>>() ?? new HashSet<string>(),
                valueComparer: PhoneNumbers_ValueComparer
            );

        builder.Property(e => e.Name)
            .HasMaxLength(150);

        builder.Property(e => e.Description)
            .HasMaxLength(300);
    }
}
