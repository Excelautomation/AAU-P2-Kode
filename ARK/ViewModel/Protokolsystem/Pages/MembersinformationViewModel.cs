using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using ARK.Model;
using ARK.Model.DB;
using ARK.View.Protokolsystem.Additional;
using ARK.ViewModel.Base.Interfaces;
using ARK.ViewModel.Protokolsystem.Additional;

namespace ARK.ViewModel.Protokolsystem.Pages
{
    internal class MembersinformationViewModel : ProtokolsystemContentViewModelBase
    {
        private FrameworkElement _additionalInfoPage;

        private DateTime _latestData;

        private IEnumerable<Member> _members;

        private IEnumerable<Member> _membersFiltered;

        private Member _selectedMember;

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

            ParentDetached +=
                (sender, args) => { ProtocolSystem.KeyboardTextChanged -= ProtocolSystem_KeyboardTextChanged; };
        }

        public IInfoContainerViewModel GetInfoContainerViewModel
        {
            get
            {
                return Parent as IInfoContainerViewModel;
            }
        }

        public MembersInformationAdditionalInfoViewModel Info
        {
            get
            {
                return InfoPage.DataContext as MembersInformationAdditionalInfoViewModel;
            }
        }

        public FrameworkElement InfoPage
        {
            get
            {
                return _additionalInfoPage ?? (_additionalInfoPage = new MembersInformationAdditionalInfo());
            }
        }

        // Properties
        public IEnumerable<Member> MembersFiltered
        {
            get
            {
                return _membersFiltered;
            }

            set
            {
                _membersFiltered = value;
                Notify();
            }
        }

        public Member SelectedMember
        {
            get
            {
                return _selectedMember;
            }

            set
            {
                _selectedMember = value;
                Notify();
                UpdateInfo();
            }
        }

        // Methods
        public void Sort(Func<Member, string> predicate)
        {
            MembersFiltered = MembersFiltered.OrderBy(predicate).ToList();
        }

        private void LoadMembers()
        {
            if (MembersFiltered == null || (DateTime.Now - _latestData).TotalHours > 1)
            {
                _latestData = DateTime.Now;

                _members = new List<Member>(db.Member).Select(
                    x =>
                        {
                            x.FirstName = x.FirstName.Trim();
                            return x;
                        }).OrderBy(x => x.FirstName).ToList();
            }
        }

        private void ProtocolSystem_KeyboardTextChanged(object sender, KeyboardEventArgs e)
        {
            MembersFiltered = string.IsNullOrEmpty(e.Text)
                                  ? _members
                                  : _members.Where(member => member.Filter(e.Text)).ToList();
        }

        private void UpdateInfo()
        {
            Info.SelectedMember = SelectedMember;

            GetInfoContainerViewModel.ChangeInfo(InfoPage, Info);
        }
    }
}