using System;
using System.Linq;

namespace TestNetFramework
{
    public static class FormatString
    {
        public static void Run()
        {
            var stringToFormat = GetArgument("String to format");
            var argumentsCount = int.Parse(GetArgument("Count of arguments"));
            var arguments = Enumerable
                .Range(0, argumentsCount)
                .Select(i => GetArgument("Argument " + i))
                .ToArray();

            Console.WriteLine();
            // ReSharper disable once CoVariantArrayConversion
            Console.WriteLine(stringToFormat, arguments);
        }

        private static string GetArgument(string argumentName)
        {
            Console.Write(argumentName + ": ");
            return Console.ReadLine();
        }
    }
}