using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Extensions;
using ARK.View.Protokolsystem.Additional;
using ARK.ViewModel.Base.Interfaces;
using ARK.ViewModel.Protokolsystem.Additional;

namespace ARK.ViewModel.Protokolsystem.Pages
{
    internal class MembersinformationViewModel : ProtokolsystemContentViewModelBase
    {
        private IEnumerable<Member> _membersFiltered;
        private IEnumerable<Member> _members;

        private Member _selectedMember;

        private FrameworkElement _additionalInfoPage;
        private DateTime _latestData;

        private DbArkContext db;

        // Constructor
        public MembersinformationViewModel()
        {
            db = DbArkContext.GetDbContext();

            ParentAttached += (sender, args) =>
            {
                // Load data
                LoadMembers();
                MembersFiltered = _members;

                // Set selected member
                SelectedMember = MembersFiltered.First();

                // Setup keyboard listener
                ProtocolSystem.KeyboardTextChanged += ProtocolSystem_KeyboardTextChanged;

                // Update info
                UpdateInfo();
            };

            ParentDetached += (sender, args) =>
            {
                ProtocolSystem.KeyboardTextChanged -= ProtocolSystem_KeyboardTextChanged;
            };
        }

        private void LoadMembers()
        {
            if (MembersFiltered == null || (DateTime.Now - _latestData).TotalHours > 1)
            {
                _latestData = DateTime.Now;

                _members = new List<Member>(db.Member)
                    .Select(x => { x.FirstName = x.FirstName.Trim(); return x; })
                    .OrderBy(x => x.FirstName)
                    .ToList();
            }
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
            set { _selectedMember = value; Notify(); UpdateInfo(); }
        }

        public FrameworkElement InfoPage
        {
            get { return _additionalInfoPage ?? (_additionalInfoPage = new MembersInformationAdditionalInfo()); }
        }

        public MembersInformationAdditionalInfoViewModel Info
        {
            get { return InfoPage.DataContext as MembersInformationAdditionalInfoViewModel; }
        }

        public IInfoContainerViewModel GetInfoContainerViewModel
        {
            get { return Parent as IInfoContainerViewModel; }
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