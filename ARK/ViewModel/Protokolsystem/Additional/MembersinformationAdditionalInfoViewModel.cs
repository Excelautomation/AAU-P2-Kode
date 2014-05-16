using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;

namespace ARK.ViewModel.Protokolsystem.Additional
{
    public class MembersInformationAdditionalInfoViewModel : ContentViewModelBase
    {
        // Fields
        #region Fields

        private Member _selectedMember;

        #endregion

        // Properties
        #region Public Properties

        public Member SelectedMember
        {
            get
            {
                return this._selectedMember;
            }

            set
            {
                this._selectedMember = value;
                this.Notify();
            }
        }

        #endregion
    }
}