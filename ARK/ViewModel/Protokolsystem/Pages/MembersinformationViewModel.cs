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
        #region Fields

        private FrameworkElement _additionalInfoPage;

        private DateTime _latestData;

        private IEnumerable<Member> _members;

        private IEnumerable<Member> _membersFiltered;

        private Member _selectedMember;

        private DbArkContext db;

        #endregion

        // Constructor
        #region Constructors and Destructors

        public MembersinformationViewModel()
        {
            this.db = DbArkContext.GetDbContext();

            this.ParentAttached += (sender, args) =>
                {
                    // Load data
                    this.LoadMembers();
                    this.MembersFiltered = this._members;

                    // Set selected member
                    this.SelectedMember = this.MembersFiltered.First();

                    // Setup keyboard listener
                    this.ProtocolSystem.KeyboardTextChanged += this.ProtocolSystem_KeyboardTextChanged;

                    // Update info
                    this.UpdateInfo();
                };

            this.ParentDetached +=
                (sender, args) =>
                    {
                        this.ProtocolSystem.KeyboardTextChanged -= this.ProtocolSystem_KeyboardTextChanged;
                    };
        }

        #endregion

        #region Public Properties

        public IInfoContainerViewModel GetInfoContainerViewModel
        {
            get
            {
                return this.Parent as IInfoContainerViewModel;
            }
        }

        public MembersInformationAdditionalInfoViewModel Info
        {
            get
            {
                return this.InfoPage.DataContext as MembersInformationAdditionalInfoViewModel;
            }
        }

        public FrameworkElement InfoPage
        {
            get
            {
                return this._additionalInfoPage ?? (this._additionalInfoPage = new MembersInformationAdditionalInfo());
            }
        }

        // Properties
        public IEnumerable<Member> MembersFiltered
        {
            get
            {
                return this._membersFiltered;
            }

            set
            {
                this._membersFiltered = value;
                this.Notify();
            }
        }

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
                this.UpdateInfo();
            }
        }

        #endregion

        // Methods
        #region Public Methods and Operators

        public void Sort(Func<Member, string> predicate)
        {
            this.MembersFiltered = this.MembersFiltered.OrderBy(predicate).ToList();
        }

        #endregion

        #region Methods

        private void LoadMembers()
        {
            if (this.MembersFiltered == null || (DateTime.Now - this._latestData).TotalHours > 1)
            {
                this._latestData = DateTime.Now;

                this._members = new List<Member>(this.db.Member).Select(
                    x =>
                        {
                            x.FirstName = x.FirstName.Trim();
                            return x;
                        }).OrderBy(x => x.FirstName).ToList();
            }
        }

        private void ProtocolSystem_KeyboardTextChanged(object sender, KeyboardEventArgs e)
        {
            this.MembersFiltered = string.IsNullOrEmpty(e.Text)
                                       ? this._members
                                       : this._members.Where(member => member.Filter(e.Text)).ToList();
        }

        private void UpdateInfo()
        {
            this.Info.SelectedMember = this.SelectedMember;

            this.GetInfoContainerViewModel.ChangeInfo(this.InfoPage, this.Info);
        }

        #endregion
    }
}