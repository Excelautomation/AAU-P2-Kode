using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Model;

namespace ARK.Extensions
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
