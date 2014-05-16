namespace ARK.Model.Extensions
{
    public static class BoatExtention
    {
        #region Public Methods and Operators

        public static bool Filter(this Boat boat, string searchText)
        {
            return boat.Name.ContainsCaseInsensitive(searchText);
        }

        #endregion
    }
}