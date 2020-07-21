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
        public void ConvertToBin(byte decimalValue, string binaryString)
        {
            var subject = GetSubject();

            var result = subject.ConvertToBin(decimalValue);

            Assert.Equal(binaryString, result);
        }

        private BinaryOperations GetSubject()
        {
            return new BinaryOperations();
        }
    }
}