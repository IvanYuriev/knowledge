using System;
using System.Linq;

namespace AsyncAwait
{
    public static class ArgsParser
    {
        public static int GetInt32OrDefault(this string[] args, int index, int defaultValue)
        {
            if (args.Length <= index)
            {
                return defaultValue;
            }
            if(!Int32.TryParse(args[index], out int value))
            {
                Console.WriteLine($"Can't parse value {args[index]}, using default value {defaultValue}");
                return defaultValue;
            }
            return value;
        }

        public static bool Contains(this string[] args, string value)
        {
            return args.Any(x => value.Equals(x, StringComparison.OrdinalIgnoreCase));
        }
    }
}