namespace SMS.Net;

using SMS.Net.Channel;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// extensions over list data types
/// </summary>
public static class ChannelDataExtensions
{
    /// <summary>
    /// get the data with the given key.
    /// </summary>
    /// <param name="data">the source list.</param>
    /// <param name="key">the data key</param>
    /// <returns>the <see cref="ChannelData"/> instance</returns>
    public static bool TryGetData<TValue>(this IEnumerable<ChannelData> data, string key, out TValue value)
    {
        var channelData = data.GetData(key);
        if (channelData.HasValue && !channelData.Value.IsEmpty())
        {
            value = channelData.Value.GetValue<TValue>();
            return true;
        }

        value = default!;
        return false;
    }

    /// <summary>
    /// get the data with the given key.
    /// </summary>
    /// <param name="data">the source list.</param>
    /// <param name="key">the data key</param>
    /// <returns>the <see cref="ChannelData"/> instance</returns>
    public static TValue GetData<TValue>(this IEnumerable<ChannelData> data, string key, TValue @default)
    {
        var channelData = data.GetData(key);
        if (channelData.HasValue && !channelData.Value.IsEmpty())
            return channelData.Value.GetValue<TValue>();

        return @default;
    }

    /// <summary>
    /// get the data with the given key.
    /// </summary>
    /// <param name="data">the source list.</param>
    /// <param name="key">the data key</param>
    /// <returns>the <see cref="ChannelData"/> instance</returns>
    public static ChannelData? GetData(this IEnumerable<ChannelData> data, string key)
        => data.FirstOrDefault(e => e.Key == key);

    /// <summary>
    /// get the value
    /// </summary>
    /// <typeparam name="TValue">the type of the value</typeparam>
    /// <returns>the value instance</returns>
    public static TValue GetValue<TValue>(this ChannelData? channelData, TValue @default)
    {
        if (channelData.HasValue && !channelData.Value.IsEmpty())
            return channelData.Value.GetValue<TValue>();

        return @default;
    }
}