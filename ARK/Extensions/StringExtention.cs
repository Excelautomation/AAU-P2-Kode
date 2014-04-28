namespace ARK.Extensions
{
    public static class StringExtention
    {
        public static bool ContainsCaseInsensitive(this string searchText, string value)
        {
            return searchText.ToLower().Contains(value.ToLower());
        }
    }
}