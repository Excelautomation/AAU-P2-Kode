using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ARK.Model;

namespace ARK.ViewModel.Protokolsystem
{
    public class MemberViewModel : ViewModelBase
    {
        private Member _member;
        private bool _visible;

        public MemberViewModel(Member member) : this (member, true)
        { }

        public MemberViewModel(Member member, bool visible)
        {
            Member = member;
            Visible = visible;
        }

        public Member Member
        {
            get { return _member; }
            set { _member = value;
                Notify();
            }
        }

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; Notify(); }
        }
    }
}
