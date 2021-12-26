namespace SMS.Net
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Phone number
    /// </summary>
    public sealed class PhoneNumber : IEquatable<PhoneNumber>, IEquatable<string>
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

        /// <summary>
        /// Add implicit constructor for PhoneNumber to make it assignable from string
        /// </summary>
        /// <param name="number">Phone number</param>
        /// <returns></returns>
        public static implicit operator string(PhoneNumber number) => number.ToString();

        /// <inheritdoc/>
        public override string ToString() => _number;

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is string stringValue) Equals(stringValue);
            if (obj is PhoneNumber number) Equals(number);
            return false;
        }

        /// <inheritdoc/>
        public bool Equals(string other)
            => other is not null && other.Equals(_number, StringComparison.OrdinalIgnoreCase);

        /// <inheritdoc/>
        public bool Equals(PhoneNumber other) 
            => other is not null && other.ToString().Equals(_number, StringComparison.OrdinalIgnoreCase);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 144377059;
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_number);
                return hashCode;
            }
        }

        /// <inheritdoc/>
        public static bool operator ==(PhoneNumber left, PhoneNumber right) => EqualityComparer<PhoneNumber>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(PhoneNumber left, PhoneNumber right) => !(left == right);
    }
}
