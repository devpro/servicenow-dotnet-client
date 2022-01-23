using System;

namespace RabbidsIncubator.ServiceNowClient.Application.Generators.Extensions
{
    public static class StringExtensions
    {
        // limitation: Generator project must be netstandard2.0 and don't support .NET 8
        // ee: https://stackoverflow.com/questions/4135317/make-first-letter-of-a-string-upper-case-with-maximum-performance

        /// <summary>
        /// Set first character of the string to upper case.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null:
                    throw new ArgumentNullException(nameof(input));
                case "":
                    return string.Empty;
                default:
                    return input[0].ToString().ToUpper() + input.Substring(1);
            }
        }

        /// <summary>
        /// Set first character of the string to lower case.
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string FirstCharToLower(this string input)
        {
            switch (input)
            {
                case null:
                    throw new ArgumentNullException(nameof(input));
                case "":
                    return string.Empty;
                default:
                    return input[0].ToString().ToLower() + input.Substring(1);
            }
        }
    }
}
