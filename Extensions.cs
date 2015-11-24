using System.Text.RegularExpressions;

namespace Warps
{
    public static class Extensions
    {
        // Sanitize strings with binary control characters 0x00-0x1f.
        public static string Sanitze(this string value)
        {
            if (value == null)
                return null;
            return Regex.Replace(value, @"([\u0000-\u001F])+", " ");
        }
    }
}
