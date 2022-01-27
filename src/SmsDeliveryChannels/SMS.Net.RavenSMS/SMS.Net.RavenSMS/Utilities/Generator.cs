namespace SMS.Net.Channel.RavenSMS.Utilities;

/// <summary>
/// use this class to generate unique ids
/// </summary>
[System.Diagnostics.DebuggerStepThrough]
internal static partial class Generator
{
    /// <summary>
    /// generate a unique tread safe id
    /// </summary>
    /// <returns>a unique string id</returns>
    public static string GenerateUniqueId() => GenerateId(Interlocked.Increment(ref _lastId));

    /// <summary>
    /// generate a unique tread safe id with a prefix
    /// </summary>
    /// <param name="prefix">the prefix value</param>
    /// <returns>a unique string id</returns>
    public static string GenerateUniqueId(string prefix) 
        => $"{prefix}_{GenerateId(Interlocked.Increment(ref _lastId))}";
}

/// <summary>
/// a partial part for <see cref="Generator"/>
/// </summary>
internal static partial class Generator
{
    private static readonly string _encode_32_Chars = "0123456789abcdefghijklmnopqrstuv";

    private static long _lastId = DateTime.UtcNow.Ticks;

    private static readonly ThreadLocal<char[]> _buffer = new(() => new char[13]);

    private static string GenerateId(long id)
    {
        var buffer = _buffer.Value ?? new char[13];

        buffer[0] = 'r';
        buffer[1] = _encode_32_Chars[(int)(id >> 55) & 31];
        buffer[2] = _encode_32_Chars[(int)(id >> 50) & 31];
        buffer[3] = _encode_32_Chars[(int)(id >> 45) & 31];
        buffer[4] = _encode_32_Chars[(int)(id >> 40) & 31];
        buffer[5] = _encode_32_Chars[(int)(id >> 35) & 31];
        buffer[6] = _encode_32_Chars[(int)(id >> 30) & 31];
        buffer[7] = _encode_32_Chars[(int)(id >> 25) & 31];
        buffer[8] = _encode_32_Chars[(int)(id >> 20) & 31];
        buffer[9] = _encode_32_Chars[(int)(id >> 15) & 31];
        buffer[10] = _encode_32_Chars[(int)(id >> 10) & 31];
        buffer[11] = _encode_32_Chars[(int)(id >> 5) & 31];
        buffer[12] = _encode_32_Chars[(int)id & 31];

        return new string(buffer, 0, buffer.Length).ToLower();
    }
}
