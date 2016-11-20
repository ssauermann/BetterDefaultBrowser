namespace BetterDefaultBrowser.Lib.Helpers
{
    internal static class Validator
    {
        public static bool IsStringMissing(string value)
        {
            return
                string.IsNullOrEmpty(value) ||
                value.Trim() == string.Empty;
        }
    }
}
