namespace SFramework.Extension
{
    public static class StringExtension
    {
        public static T ToEnum<T>(this string value, T defaultValue)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            T result;
            return Enum.TryParse<T>(value, true, out result) ? result : defaultValue;
        }
        
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