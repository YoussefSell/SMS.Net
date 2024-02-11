namespace SMS.Net.Exceptions
{
    using System;

    /// <summary>
    /// exception thrown when a required options value is not specified
    /// </summary>
    [Serializable]
    public class RequiredOptionValueNotSpecifiedException<TOptions> : Exception
    {
        /// <inheritdoc/>
        public RequiredOptionValueNotSpecifiedException(string optionsName, string message) : base(message)
        {
            OptionsName = optionsName;
        }

        /// <inheritdoc/>
        protected RequiredOptionValueNotSpecifiedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        /// <summary>
        /// the option object type
        /// </summary>
        public Type OptionsType => typeof(TOptions);

        /// <summary>
        /// option parameter name
        /// </summary>
        public string OptionsName { get; }
    }
}
