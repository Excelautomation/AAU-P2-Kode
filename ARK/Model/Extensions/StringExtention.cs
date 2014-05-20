namespace ARK.Model.Extensions
{
    public static class StringExtention
    {
        public static bool ContainsCaseInsensitive(this string searchText, string value)
        {
            return searchText != null && searchText.ToLower().Contains(value.ToLower());
        }
    }
}