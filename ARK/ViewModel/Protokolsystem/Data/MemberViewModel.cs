using ARK.Model;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Protokolsystem.Data
{
    public class MemberViewModel : ViewModelBase
    {
        private bool _isResponsible;

        private Member _member;

        private bool _visible;

        public MemberViewModel(Member member)
            : this(member, true)
        {
        }

        public MemberViewModel(Member member, bool visible)
        {
            Member = member;
            Visible = visible;
        }

        public bool IsResponsible
        {
            get
            {
                return _isResponsible;
            }

            set
            {
                _isResponsible = value;
                Notify();
            }
        }

        public Member Member
        {
            get
            {
                return _member;
            }

            set
            {
                _member = value;
                Notify();
            }
        }

        public bool Visible
        {
            get
            {
                return _visible;
            }

            set
            {
                _visible = value;
                Notify();
            }
        }

        // Is responsible for longtrip (not used in begintrip)
    }
}