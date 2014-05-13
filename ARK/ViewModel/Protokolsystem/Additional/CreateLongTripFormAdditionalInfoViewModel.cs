using System.Windows.Input;
using ARK.ViewModel.Protokolsystem.Data;

namespace ARK.ViewModel.Protokolsystem.Additional
{
    public class CreateLongTripFormAdditionalInfoViewModel : MemberSelectorViewModel
    {
        private bool _selectResponsible;
        private MemberViewModel _responsibleMember;

        public bool SelectResponsible
        {
            get { return _selectResponsible; }
            set
            {
                _selectResponsible = value;
                Notify();
            }
        }

        public MemberViewModel ResponsibleMember
        {
            get { return _responsibleMember; }
            set
            {
                if (_responsibleMember != null)
                    _responsibleMember.IsResponsible = false;
                
                _responsibleMember = value;

                if (_responsibleMember != null)
                    _responsibleMember.IsResponsible = true;
                
                Notify();
            }
        }

        public ICommand ResponsibleMemberClick
        {
            get { return GetCommand<object>(e => { SelectResponsible = !SelectResponsible; }); }
        }

        public ICommand MemberClicked
        {
            get
            {
                return GetCommand<MemberViewModel>(member =>
                {
                    if (SelectResponsible)
                        ResponsibleMember = member;
                    else
                        RemoveMember.Execute(member);
                });
            }
        }
    }
}