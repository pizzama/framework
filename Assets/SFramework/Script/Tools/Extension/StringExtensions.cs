using System;

namespace SFramework.Extension
{
    public static class StringExtensions
    {
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        // public static T ToEnum<T>(this string value, T defaultValue)
        //     where T : IComparable
        // {
        //     if (string.IsNullOrEmpty(value))
        //     {
        //         return defaultValue;
        //     }

        //     T result;
        //     return Enum.TryParse<T>(value, true, out result) ? result : defaultValue;
        // }

        public static string RemoveInvalidateChars(this string name)
        {
            return name.Replace("/", "_")
                .Replace("@", "")
                .Replace("!", "")
                .Replace(" ", "_")
                .Replace("__", "_")
                .Replace("__", "_")
                .Replace("__", "_")
                .Replace("&", "")
                .Replace("-", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("#", "")
                .Replace(".", "_");
        }
    }
}
