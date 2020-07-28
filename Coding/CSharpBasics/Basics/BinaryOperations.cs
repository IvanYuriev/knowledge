using System;

namespace Basics
{
    public class BinaryOperations
    {
        public string ByteToBin(byte value)
        {
            // Built-in implementation:
            //return Convert.ToString(value, 2);
            
            // 6 = b 0000 0110
            // 128 b 1000 0000
            var result = new char[] 
            { 
                '0', '0', '0', '0',   
                '0', '0', '0', '0' 
            };
            var position = 0;
            for (byte i = 1 << 7; i > 0; i /= 2)
            {
                if ((i & value) == i) //is it working?
                {
                    result[position] = '1';
                }
                position++;
            }

            return new string(result);
        }

        public string UInt32ToBin(uint value)
        {
            //let's try another approach - the classical one
            var size = sizeof(uint) * 8;
            return String.Create(size, value, (chars, value) =>
            {
                var position = size - 1;
                while(value > 0)
                {
                    chars[position--] = (char)((value % 2) + 48); //48 ascii is a 0 character
                    value = value / 2;
                }

                for(int i = position; i >= 0; i--) chars[i] = '0';
            });
        }

        public void SubtractionIsReplacedWithAddition()
        {
            var a = 0b0000_0101; // 5 DEC
            var b = ~a;          // -5 -1 = -6 DEC
            var c = a + b;       // -1 DEC: 1111 1111

            Print(a);
            Print(b);
            Print(c);
        }

        public void PrintAllNumbers(int start, int end)
        {
            if (start > end)
            {
                throw new ArgumentOutOfRangeException("Start should be lower or equal end");
            }
            
            Console.WriteLine("Iterating:");
            for(int i = start; i <= end; i++)
            {
                Print(i);
            }
        }

        private static void Print(int value)
        {
            Console.WriteLine($"{value,5}: {Convert.ToString(value, 2).PadLeft(32, '0')}");
        }
    }
}
