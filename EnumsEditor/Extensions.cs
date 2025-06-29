using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace EnumsEditor
{
    public static class Extensions
    {
        public static bool IsValidCSharpOperatorName(this string value)
        {
            bool containsOnlyLettersAndNumbersAndDontStartsFromNumber = Regex.IsMatch(value, "^[a-zA-Z][a-zA-Z0-9]*$");
            return containsOnlyLettersAndNumbersAndDontStartsFromNumber;
        }

        public static string ListToString<T>(this IList<T> list)
        {
            StringBuilder builder = new();
            for (int i = 0; i < list.Count; i++)
                builder.Append($"{i}: ").Append(list[i]).Append(Environment.NewLine);
            return builder.ToString();
        }
    }
}