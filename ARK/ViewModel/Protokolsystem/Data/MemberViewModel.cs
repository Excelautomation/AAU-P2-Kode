using ARK.Model;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Protokolsystem.Data
{
    public class MemberViewModel : ViewModelBase
    {
        #region Fields

        private bool _isResponsible;

        private Member _member;

        private bool _visible;

        #endregion

        #region Constructors and Destructors

        public MemberViewModel(Member member)
            : this(member, true)
        {
        }

        public MemberViewModel(Member member, bool visible)
        {
            this.Member = member;
            this.Visible = visible;
        }

        #endregion

        #region Public Properties

        public bool IsResponsible
        {
            get
            {
                return this._isResponsible;
            }

            set
            {
                this._isResponsible = value;
                this.Notify();
            }
        }

        public Member Member
        {
            get
            {
                return this._member;
            }

            set
            {
                this._member = value;
                this.Notify();
            }
        }

        public bool Visible
        {
            get
            {
                return this._visible;
            }

            set
            {
                this._visible = value;
                this.Notify();
            }
        }

        #endregion

        // Is responsible for longtrip (not used in begintrip)
    }
}