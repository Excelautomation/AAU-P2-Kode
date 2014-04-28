using ARK.Model;

namespace ARK.Extensions
{
    public static class LongDistanceFormExtention
    {
        public static bool FilterLongDistanceForm(this LongDistanceForm longDistanceForm, string searchText)
        {
            return longDistanceForm.Boat.FilterBoat(searchText) ||
                   longDistanceForm.Text.ContainsCaseInsensitive(searchText);
        }
    }
}