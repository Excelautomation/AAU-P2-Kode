namespace ARK.Model.Extensions
{
    public static class BoatExtention
    {
        public static bool Filter(this Boat boat, string searchText)
        {
            return boat.Name.ContainsCaseInsensitive(searchText);
        }
    }
}