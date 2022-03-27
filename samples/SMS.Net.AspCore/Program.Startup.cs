using Hangfire.Storage.MySql;
using System.Transactions;

public static partial class Program
{
    public static IGlobalConfiguration UseMariaDb(this IGlobalConfiguration configuration, string connectionString)
    {
        var options = new MySqlStorageOptions
        {
            TransactionIsolationLevel = IsolationLevel.ReadCommitted,
            QueuePollInterval = TimeSpan.FromSeconds(15),
            JobExpirationCheckInterval = TimeSpan.FromHours(1),
            CountersAggregateInterval = TimeSpan.FromMinutes(5),
            PrepareSchemaIfNecessary = true,
            DashboardJobListLimit = 50000,
            TransactionTimeout = TimeSpan.FromMinutes(1),
            TablesPrefix = "Hangfire"
        };

        var storage = new MySqlStorage(connectionString, options);
        configuration.UseStorage(storage);
        return configuration;
    }
}
