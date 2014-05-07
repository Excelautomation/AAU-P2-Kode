using ARK.Model;
using ARK.Model.DB;
using ARK.Protokolsystem.Pages;
using ARK.View.Protokolsystem.Additional;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;
using ARK.ViewModel.Protokolsystem.Additional;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ARK.ViewModel.Protokolsystem
{
    internal class MembersinformationViewModel : ProtokolsystemContentViewModelBase
    {
        private List<Member> _members;

        private Member _selectedMember;

        private FrameworkElement _additionalInfoPage;

        // Constructor
        public MembersinformationViewModel()
        {
            var db = DbArkContext.GetDbContext();

            // Load data
            _members = new List<Member>(db.Member)
                .Select(x => { x.FirstName = x.FirstName.Trim(); return x; })
                .OrderBy(x => x.FirstName)
                .ToList();

            ParentAttached += (sender, args) => UpdateInfo();
        }

        // Properties
        public List<Member> Members
        {
            get { return _members; }
            set { _members = value; Notify(); }
        }

        public Member SelectedMember
        {
            get { return _selectedMember; }
            set { _selectedMember = value; Notify(); }
        }

        public FrameworkElement InfoPage
        {
            get { return _additionalInfoPage ?? (_additionalInfoPage = new MembersInformationAdditionalInfo()); }
        }

        public MembersinformationAdditionalInfoViewModel Info
        {
            get { return InfoPage.DataContext as MembersinformationAdditionalInfoViewModel; }
        }

        public IInfoContainerViewModel GetInfoContainerViewModel
        {
            get { return Parent as IInfoContainerViewModel; }
        }

        public ICommand MemberSelectionChanged
        {
            get
            {
                return GetCommand<Member>(e =>
                {
                    SelectedMember = e;
                    UpdateInfo();
                });
            }
        }

        // Methods
        public void Sort(Func<Member, string> predicate)
        {
            Members = Members.OrderBy(predicate).ToList();
        }

        private void UpdateInfo()
        {
            Info.SelectedMember = SelectedMember;

            GetInfoContainerViewModel.ChangeInfo(InfoPage, Info);
        }
    }
}