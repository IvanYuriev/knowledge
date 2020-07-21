namespace Basics
{
    public class BinaryOperations
    {
        public string ConvertToBin(byte value)
        {
            // 6 = b 0000 0110
            // 128 b 1000 0000
            var result = new char[] { '0', '0', '0', '0',   '0', '0', '0', '0' };
            var position = 0;
            for (byte i = 1 << 7; i > 0; i /= 2)
            {
                if ((i & value) == i) //is it working?
                {
                    result[position] = '1';
                }
                position++;
            }

            //should be reversed?
            return new string(result);
        }
    }
}
