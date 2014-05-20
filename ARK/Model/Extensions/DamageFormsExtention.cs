namespace ARK.Model.Extensions
{
    public static class DamageFormsExtention
    {
        public static bool Filter(this DamageForm damageForm, string searchText)
        {
            return damageForm.Boat.Filter(searchText) || damageForm.Description.ContainsCaseInsensitive(searchText);
        }
    }
}