namespace SMS.Net
{
    using System;

    /// <summary>
    /// Phone number
    /// </summary>
    public class PhoneNumber
    {
        private readonly string _number;

        /// <summary>
        /// Create a new PhoneNumber
        /// </summary>
        /// <param name="number">Phone number</param>
        public PhoneNumber(string number)
        {
            if (number is null)
                throw new ArgumentNullException(nameof(number));

            if (string.IsNullOrEmpty(number))
                throw new ArgumentException("the given phone number is empty", nameof(number));

            _number = number;
        }

        /// <summary>
        /// Add implicit constructor for PhoneNumber to make it assignable from string
        /// </summary>
        /// <param name="number">Phone number</param>
        /// <returns></returns>
        public static implicit operator PhoneNumber(string number) 
            => new PhoneNumber(number);

        /// <inheritdoc/>
        public override string ToString() => _number;
    }
}
