namespace ARK.Model.Extensions
{
    public static class MembersExtention
    {
        #region Public Methods and Operators

        public static bool Filter(this Member member, string searchText)
        {
            return member.FirstName.ContainsCaseInsensitive(searchText)
                   || member.LastName.ContainsCaseInsensitive(searchText)
                   || member.MemberNumber.ToString().ContainsCaseInsensitive(searchText);
        }

        #endregion
    }
}