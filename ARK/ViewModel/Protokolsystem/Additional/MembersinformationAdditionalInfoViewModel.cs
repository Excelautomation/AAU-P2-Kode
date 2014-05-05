using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.ViewModel.Protokolsystem.Additional
{
    class MembersinformationAdditionalInfoViewModel : ContentViewModelBase
    {
        // Fields
        private Member _selectedMember;

        // Properties
        public Member SelectedMember
        {
            get { return _selectedMember; }
            set { _selectedMember = value; Notify(); }
        }
    }
}
