using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IdentityGuard.Core.Extensions
{
    public static class QueryExtensions
    {
        public static string ResolveQueryParameters(this string query)
        {
            var regex = new Regex(@"{(.*?)}");

            var dynamicElements = regex.Matches(query)
                .Select(m => m.Value.Trim())
                .ToList();

            var result = query;
            foreach (var element in dynamicElements)
            {
                var resolved = ResolveDynamicElement(element);
                result = result.Replace(element, resolved);
            }

            return result;
        }

        private static string ResolveDynamicElement(string element)
        {
            var agoRegex = new Regex(@"ago\((.*?)\)");

            var agoCommands = agoRegex.Matches(element)
                .Select(m => m.Value.Trim())
                .ToList();

            if (!agoCommands.Any()) return element;

            return ResolveAgoCommand(agoCommands.First());
        }

        private static string ResolveAgoCommand(string command)
        {
            var agoRegex = new Regex(@"\((.*?)\)");

            var agoCommands = agoRegex.Matches(command)
                .Select(m => m.Value.Trim())
                .ToList();

            if (!agoCommands.Any()) return command;

            var input = agoCommands.First().Replace("(", "").Replace(")", "");

            return ParseDate(input);
        }

        private static string ParseDate(string input)
        {
            if (input.EndsWith("d"))
            {
                var value = int.Parse(input.Replace("d", ""));
                return DateTime.Now.AddDays(-value).ToString("yyyy-MM-dd");
            }
            if (input.EndsWith("m"))
            {
                var value = int.Parse(input.Replace("m", ""));
                return DateTime.Now.AddMonths(-value).ToString("yyyy-MM-dd");
            }
            if (input.EndsWith("y"))
            {
                var value = int.Parse(input.Replace("y", ""));
                return DateTime.Now.AddYears(-value).ToString("yyyy-MM-dd");
            }

            return input;
        }
    }
}
