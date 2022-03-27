namespace SMS.Net
{
    using System;

    internal static class EnvVariable
    {
        public static string Load(string name)
        {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process)
                ?? throw new ArgumentNullException(nameof(name), $"failed to load the EnvironmentVariable with name [{name}]");
        }
    }
}
