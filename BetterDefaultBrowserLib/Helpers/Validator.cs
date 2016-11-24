namespace BetterDefaultBrowser.Lib.Helpers
{
    public static class Validator
    {
        public static bool IsStringMissing(string value)
        {
            return
                string.IsNullOrEmpty(value) ||
                value.Trim() == string.Empty;
        }
    }
}
