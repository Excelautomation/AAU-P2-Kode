using System.Windows.Input;

using ARK.ViewModel.Protokolsystem.Data;

namespace ARK.ViewModel.Protokolsystem.Additional
{
    public class CreateLongTripFormAdditionalInfoViewModel : MemberSelectorViewModel
    {
        private MemberViewModel _responsibleMember;

        private bool _selectResponsible = true;

        public ICommand MemberClicked
        {
            get
            {
                return GetCommand(
                    member =>
                        {
                            var memberVm = (MemberViewModel)member;

                            if (SelectResponsible)
                            {
                                ResponsibleMember = memberVm;
                            }
                            else
                            {
                                memberVm.IsResponsible = false;
                                RemoveMember.Execute(memberVm);
                            }
                        });
            }
        }

        public MemberViewModel ResponsibleMember
        {
            get
            {
                return _responsibleMember;
            }

            set
            {
                if (_responsibleMember != null)
                {
                    _responsibleMember.IsResponsible = false;
                }

                _responsibleMember = value;

                if (_responsibleMember != null)
                {
                    _responsibleMember.IsResponsible = true;
                }

                Notify();
            }
        }

        public ICommand ResponsibleMemberClick
        {
            get
            {
                return GetCommand(() => { SelectResponsible = true; });
            }
        }

        public ICommand DeleteMemberClick
        {
            get
            {
                return GetCommand(() => { SelectResponsible = false; });
            }
        }

        public bool SelectResponsible
        {
            get
            {
                return _selectResponsible;
            }

            set
            {
                _selectResponsible = value;
                Notify();
            }
        }
    }
}