namespace ARK.Model.Extensions
{
    public static class DamageFormsExtention
    {
        public static bool FilterDamageForms(this DamageForm damageForm, string searchText)
        {
            return damageForm.Boat.FilterBoat(searchText) ||
                   damageForm.Description.ContainsCaseInsensitive(searchText);
        }
    }
}