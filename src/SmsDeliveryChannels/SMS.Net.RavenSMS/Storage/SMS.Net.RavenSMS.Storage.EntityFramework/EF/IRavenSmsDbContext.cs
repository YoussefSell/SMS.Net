namespace SMS.Net.Channel.RavenSMS.Stores.EntityFramework;

/// <summary>
/// an interface that defines the entity framework db context
/// </summary>
public interface IRavenSmsDbContext
{
    /// <summary>
    /// Creates a Microsoft.EntityFrameworkCore.DbSet`1 that can be used to query and save instances of TEntity.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity for which a set should be returned.</typeparam>
    /// <returns>A set for the given entity type.</returns>
    public DbSet<TEntity> Set<TEntity>() where TEntity : class;

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// This method will automatically call Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges
    /// to discover any changes to entity instances before saving to the underlying database.
    /// This can be disabled via Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled.
    /// Entity Framework Core does not support multiple parallel operations being run
    /// on the same DbContext instance. This includes both parallel execution of async
    /// queries and any explicit concurrent use from multiple threads. Therefore, always
    /// await async calls immediately, or use separate DbContext instances for operations
    /// that execute in parallel. See Avoiding DbContext threading issues for more information.
    /// </summary>
    /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
    /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
    /// <exception cref="DbUpdateConcurrencyException">
    /// A concurrency violation is encountered while saving to the database. A concurrency
    /// violation occurs when an unexpected number of rows are affected during save.
    /// This is usually because the data in the database has been modified since it was
    /// loaded into memory.
    /// </exception>
    /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
    /// <remarks>See Saving data in EF Core for more information.</remarks>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
