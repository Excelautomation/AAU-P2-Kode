using System.Windows.Input;

using ARK.ViewModel.Protokolsystem.Data;

namespace ARK.ViewModel.Protokolsystem.Additional
{
    public class CreateLongTripFormAdditionalInfoViewModel : MemberSelectorViewModel
    {
        #region Fields

        private MemberViewModel _responsibleMember;

        private bool _selectResponsible;

        #endregion

        #region Public Properties

        public ICommand MemberClicked
        {
            get
            {
                return this.GetCommand(
                    member =>
                        {
                            var memberVm = (MemberViewModel)member;

                            if (this.SelectResponsible)
                            {
                                this.ResponsibleMember = memberVm;
                            }
                            else
                            {
                                memberVm.IsResponsible = false;
                                this.RemoveMember.Execute(memberVm);
                            }
                        });
            }
        }

        public MemberViewModel ResponsibleMember
        {
            get
            {
                return this._responsibleMember;
            }

            set
            {
                if (this._responsibleMember != null)
                {
                    this._responsibleMember.IsResponsible = false;
                }

                this._responsibleMember = value;

                if (this._responsibleMember != null)
                {
                    this._responsibleMember.IsResponsible = true;
                }

                this.Notify();
            }
        }

        public ICommand ResponsibleMemberClick
        {
            get
            {
                return this.GetCommand(() => { this.SelectResponsible = !this.SelectResponsible; });
            }
        }

        public bool SelectResponsible
        {
            get
            {
                return this._selectResponsible;
            }

            set
            {
                this._selectResponsible = value;
                this.Notify();
            }
        }

        #endregion
    }
}