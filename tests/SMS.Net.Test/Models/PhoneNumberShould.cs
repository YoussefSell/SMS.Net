namespace SMS.Net.Test.Models
{
    using System;
    using Xunit;

    public class PhoneNumberShould
    {
        [Fact]
        public void ThrowIfValueIsNull()
        {
            // arrange
            // act

            // assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new PhoneNumber(null);
            });
        }

        [Fact]
        public void ThrowIfValueIsEmpty()
        {
            // arrange
            // act

            // assert
            Assert.Throws<ArgumentException>(() =>
            {
                new PhoneNumber("");
            });
        }


        [Fact]
        public void CreatePhoneNumberInstance()
        {
            // arrange
            var expected = "+212625415254";

            // act
            var phoneNumber = new PhoneNumber("+212625415254");

            // assert
            Assert.Equal(expected, phoneNumber.ToString());
        }

        [Fact]
        public void CreatePhoneNumberInstanceImlicitlyFromString()
        {
            // arrange
            var expected = "+212625415254";

            // act
            PhoneNumber phoneNumber = "+212625415254";

            // assert
            Assert.Equal(expected, phoneNumber.ToString());
        }

        [Fact]
        public void BeEqualIfHaveSameValue()
        {
            // arrange
            var phoneNumber1 = new PhoneNumber("+212625415254");
            var phoneNumber2 = new PhoneNumber("+212625415254");

            // act
            var equals = phoneNumber1 == phoneNumber2;

            // assert
            Assert.True(equals);
        }

        [Fact]
        public void BeEqualToStringIfHaveSameValue()
        {
            // arrange
            var phoneNumber1 = "+212625415254";
            var phoneNumber2 = new PhoneNumber("+212625415254");

            // act
            var equals = phoneNumber1 == phoneNumber2;

            // assert
            Assert.True(equals);
        }
    }
}
