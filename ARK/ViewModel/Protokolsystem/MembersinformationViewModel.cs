using System;
using System.Collections.Generic;
using System.Linq;
using ARK.Model;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Protokolsystem
{
    internal class MembersinformationViewModel<T> : ViewModelBase
    {
        private MembersinformationViewModel(Func<Member, T> predicate)
        {
            Sort(predicate);
        }

        public Member SelectedMember { get; set; }

        public List<Member> Members { get; set; }

        public void Sort(Func<Member, T> predicate)
        {
            Members = Members.OrderBy(predicate).ToList();
        }


    }
}