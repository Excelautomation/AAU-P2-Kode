using ARK.Model;

namespace ARK.Extensions
{
    public static class BoatExtention
    {
        public static bool FilterBoat(this Boat boat, string searchText)
        {
            return boat.Name.ContainsCaseInsensitive(searchText);
        }
    }
}