using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Model;

namespace ARK.Interfaces
{
    interface IMemberCollection<T>
    {
        Member SelectedMember { get; set; }

        IEnumerable<Member> Members { get; set; }

        void Sort(Func<Member, T> predicate);
    }
}