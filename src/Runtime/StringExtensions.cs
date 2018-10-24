using System;

namespace HotChocolate.Utilities
{
    public static class StringExtensions
    {
        public static bool EqualsOrdinal(this string x, string y)
        {
            return string.Equals(x, y, StringComparison.Ordinal);
        }
    }
}
