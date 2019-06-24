namespace Escl.Utils
{
    public static class StringExtensions
    {
        public static int ParseIntOrDefault(this string valueString, int defaultValue)
        {
            return int.TryParse(valueString, out int value) ? value : defaultValue;
        }
    }
}