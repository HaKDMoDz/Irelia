using System;

namespace Demacia.Utils
{
    public static class StringUtils
    {
        public static bool Contains(this string original, string value, StringComparison comparision)
        {
            return original.IndexOf(value, comparision) >= 0;
        }
    }
}
