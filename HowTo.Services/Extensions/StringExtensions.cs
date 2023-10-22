using System.Text.RegularExpressions;

namespace HowTo.Services.Extensions
{
    public static class StringExtensions
    {
        public static string Stuff(this string s, int interval, string value)
        {
            string pattern = $".{{{interval}}}";

            Console.WriteLine(pattern);

            var substrings = Regex.Matches(s, pattern)
                .Cast<Match>()
                .Select(m => m.Value + value)
                .ToArray();

            return String.Concat<string>(substrings) + s.Substring(substrings.Length*interval);
        }
    }
}
