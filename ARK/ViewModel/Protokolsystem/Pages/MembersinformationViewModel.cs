using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Extensions;
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
        private IEnumerable<Member> _membersFiltered;
        private readonly List<Member> _members;

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
            _membersFiltered = _members;

            ParentAttached += (sender, args) =>
            {
                UpdateInfo();

                ProtocolSystem.KeyboardTextChanged += ProtocolSystem_KeyboardTextChanged;
            };

            ParentDetached += (sender, args) =>
            {
                ProtocolSystem.KeyboardTextChanged -= ProtocolSystem_KeyboardTextChanged;
            };
        }

        private void ProtocolSystem_KeyboardTextChanged(object sender, KeyboardEventArgs e)
        {
            MembersFiltered = string.IsNullOrEmpty(e.Text) ? _members : _members.Where(member => member.Filter(e.Text)).ToList();
        }

        // Properties
        public IEnumerable<Member> MembersFiltered
        {
            get { return _membersFiltered; }
            set { _membersFiltered = value; Notify(); }
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
            MembersFiltered = MembersFiltered.OrderBy(predicate).ToList();
        }

        private void UpdateInfo()
        {
            Info.SelectedMember = SelectedMember;

            GetInfoContainerViewModel.ChangeInfo(InfoPage, Info);
        }
    }
}