using System;
using Xunit;

namespace Basics.Tests
{
    public class BinaryOperationsTests
    {
        [Theory]
        [InlineData(005, "00000101")]
        [InlineData(128, "10000000")]
        [InlineData(255, "11111111")]
        [InlineData(000, "00000000")]
        public void ByteToBin(byte decimalValue, string binaryString)
        {
            var subject = GetSubject();

            var result = subject.ByteToBin(decimalValue);

            Assert.Equal(binaryString, result);
        }

        [Theory]
        [InlineData(128)]
        [InlineData(005)]
        [InlineData(255)]
        [InlineData(000)]
        [InlineData(uint.MaxValue)]
        [InlineData(1234568)]
        public void UInt32ToBin(uint decimalValue)
        {
            var subject = GetSubject();

            var result = subject.UInt32ToBin(decimalValue);

            Assert.Equal(Convert.ToString(decimalValue, 2).PadLeft(32, '0'), result);
        }

        private BinaryOperations GetSubject()
        {
            return new BinaryOperations();
        }
    }
}