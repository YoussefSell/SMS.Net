namespace SMS.Net.Channel.RavenSMS.EntityFramework;

/// <summary>
/// extension methods for string
/// </summary>
internal static class StringExtensions
{
    /// <summary>
    /// convert the given entity to a json string.
    /// </summary>
    /// <typeparam name="TEntity">the entity type</typeparam>
    /// <param name="entity">the entity instance</param>
    /// <returns>json string</returns>
    internal static string ToJson<TEntity>(this TEntity entity)
        => System.Text.Json.JsonSerializer.Serialize(entity);

    /// <summary>
    /// convert the given json string value to an entity
    /// </summary>
    /// <typeparam name="TEntity">the entity type</typeparam>
    /// <param name="value">the string json value</param>
    /// <returns>instance of entity or null</returns>
    internal static TEntity? FromJson<TEntity>(this string value)
        => System.Text.Json.JsonSerializer.Deserialize<TEntity>(value);
}
