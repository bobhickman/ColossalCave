using System;

namespace ColossalCave.Engine.Utilities
{
    public static class StringExtensions
    {
        /// <summary>
        /// Determines if the given strings are equal, ignoring case.
        /// </summary>
        public static bool EqualsNoCase(this string s, string other)
        {
            return s != null && s.Equals(other, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines if the given string ends with a string, ignoring case.
        /// </summary>
        public static bool EndsWithNoCase(this string s, string other)
        {
            return s != null && s.EndsWith(other, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines if the given string starts with a string, ignoring case.
        /// </summary>
        public static bool StartsWithNoCase(this string s, string other)
        {
            return s != null && s.StartsWith(other, StringComparison.OrdinalIgnoreCase);
        }

    }
}
