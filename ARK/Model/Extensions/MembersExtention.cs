namespace ARK.Model.Extensions
{
    public static class MembersExtention
    {
        public static bool FilterMembers(this Member damageForm, string searchText)
        {
            return damageForm.FirstName.ContainsCaseInsensitive(searchText) ||
                   damageForm.LastName.ContainsCaseInsensitive(searchText);
        }
    }
}
