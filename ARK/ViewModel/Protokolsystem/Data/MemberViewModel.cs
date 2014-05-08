using ARK.Model;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Protokolsystem.Data
{
    public class MemberViewModel : ViewModelBase
    {
        private Member _member;
        private bool _visible;

        public MemberViewModel(Member member) : this(member, true)
        {
        }

        public MemberViewModel(Member member, bool visible)
        {
            Member = member;
            Visible = visible;
        }

        public Member Member
        {
            get { return _member; }
            set
            {
                _member = value;
                Notify();
            }
        }

        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                Notify();
            }
        }
    }
}