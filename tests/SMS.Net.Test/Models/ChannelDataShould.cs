namespace SMS.Net.Test.Models
{
    using SMS.Net.Channel;
    using Xunit;

    public class ChannelDataShould
    {
        [Fact]
        public void EqualIfHaveSameKey()
        {
            // arrange
            var other1 = new ChannelData(key: "key1", value: "this is a test");
            var other2 = new ChannelData(key: "key1", value: 12);

            // act
            var equlas = other1 == other2;

            // assert
            Assert.True(equlas);
        }

        [Fact]
        public void NotEqualIfHaveDeferentKey()
        {
            // arrange
            var other1 = new ChannelData(key: "key1", value: "this is a test");
            var other2 = new ChannelData(key: "key2", value: 12);

            // act
            var equlas = other1 == other2;

            // assert
            Assert.False(equlas);
        }

        [Fact]
        public void HaveSameHashIfTheyHaveSameKey()
        {
            // arrange
            var other1 = new ChannelData(key: "key1", value: "this is a test");
            var other2 = new ChannelData(key: "key1", value: 12);

            // act
            var equlas = other1.GetHashCode() == other2.GetHashCode();

            // assert
            Assert.True(equlas);
        }
    }
}
