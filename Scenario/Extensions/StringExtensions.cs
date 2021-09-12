using System;
namespace Scenario.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string input)
        {
            if(input == null)
            {
                throw new ArgumentNullException();
            }
            if(input == string.Empty)
            {
                return string.Empty;
            }
            return input[0].ToString().ToLower() + (input.Length > 1 ? input[1..] : string.Empty);
        }
    }
}
