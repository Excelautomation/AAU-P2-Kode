namespace ARK.Model.Extensions
{
    public static class LongDistanceFormExtention
    {
        #region Public Methods and Operators

        public static bool Filter(this LongTripForm longDistanceForm, string searchText)
        {
            return longDistanceForm.Boat.Filter(searchText)
                   || longDistanceForm.TourDescription.ContainsCaseInsensitive(searchText);
        }

        #endregion
    }
}