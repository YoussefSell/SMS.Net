namespace SMS.Net
{
    using SMS.Net.Channel;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// extensions over list data types
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// get the data with the given key.
        /// </summary>
        /// <param name="data">the source list.</param>
        /// <param name="key">the data key</param>
        /// <returns>the <see cref="ChannelData"/> instance</returns>
        public static ChannelData GetData(this IEnumerable<ChannelData> data, string key)
            => data.FirstOrDefault(e => e.Key == key);
    }
}
