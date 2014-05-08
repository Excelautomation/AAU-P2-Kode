using System.Linq;

namespace ARK.Model.Extensions
{
    public static class TripExtention
    {
        public static bool Filter(this Trip trip, string searchText)
        {
            return trip.Boat.Filter(searchText) || 
                (trip.Direction != null && trip.Direction.ContainsCaseInsensitive(searchText)) ||
                trip.Members.Any(member => member.Filter(searchText));
        }
    }
}