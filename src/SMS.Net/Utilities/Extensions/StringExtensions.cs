namespace SMS.Net.Utilities
{
    /// <summary>
    /// extension methods on the <see cref="string"/> type
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// check if the given string is not null or empty or white space
        /// </summary>
        /// <param name="value">the string value to be checked</param>
        /// <returns>true if valid, false if not</returns>
        public static bool IsValid(this string value)
            => !(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value));
    }
}
