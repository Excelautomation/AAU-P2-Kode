using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Interfaces;
using ARK.Model;

namespace ARK.ViewModel.Protokolsystem
{
    class MembersinformationViewModel<T> : ViewModelBase, IMemberCollection<T>
    {
        MembersinformationViewModel(Func<Member, T> predicate)
        {
            Sort(predicate);
        }

        public Member SelectedMember { get; set; }

        public IEnumerable<Member> Members { get; set; }

        public void Sort(Func<Member, T> predicate)
        {
            Members = Members.OrderBy(predicate).ToList();
        }
    }
}
