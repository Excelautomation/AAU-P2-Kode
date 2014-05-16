using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using ARK.Model;
using ARK.Model.DB;
using ARK.View.Protokolsystem.Additional;
using ARK.View.Protokolsystem.Pages;
using ARK.ViewModel.Protokolsystem.Additional;

namespace ARK.ViewModel.Protokolsystem.Pages
{
    internal class ViewLongTripFormViewModel : ProtokolsystemContentViewModelBase
    {
        // Fields
        #region Fields

        private FrameworkElement _infoPage;

        private List<LongTripForm> _longTripForms;

        private LongTripForm _selectedLongTripForm;

        #endregion

        // Constructor
        #region Constructors and Destructors

        public ViewLongTripFormViewModel()
        {
            var db = DbArkContext.GetDbContext();

            this.ParentAttached += (sender, e) =>
                {
                    this.LongTripForms = db.LongTripForm.ToList();

                    if (this.LongTripForms.Any())
                    {
                        this.SelectedLongTripForm = this.LongTripForms.First();
                    }

                    this.UpdateInfo();
                };
        }

        #endregion

        #region Public Properties

        public ICommand CreateLongTripForm
        {
            get
            {
                return
                    this.GetCommand(
                        () => this.ProtocolSystem.NavigateToPage(() => new CreateLongTripForm(), "OPRET NY LANGTUR"));
            }
        }

        // Props
        public List<LongTripForm> LongTripForms
        {
            get
            {
                return this._longTripForms;
            }

            set
            {
                this._longTripForms = value;
                this.Notify();
            }
        }

        public LongTripForm SelectedLongTripForm
        {
            get
            {
                return this._selectedLongTripForm;
            }

            set
            {
                this._selectedLongTripForm = value;
                this.Notify();
                this.UpdateInfo();
            }
        }

        public ICommand ViewLongTripForm
        {
            get
            {
                return
                    this.GetCommand(
                        () => this.ProtocolSystem.NavigateToPage(() => new ViewLongTripForm(), "LANGTURSBLANKETTER"));
            }
        }

        #endregion

        #region Properties

        private ViewLongTripFormAdditionalInfoViewModel Info
        {
            get
            {
                return this.InfoPage.DataContext as ViewLongTripFormAdditionalInfoViewModel;
            }
        }

        private FrameworkElement InfoPage
        {
            get
            {
                return this._infoPage ?? (this._infoPage = new ViewLongTripFormAdditionalInfo());
            }
        }

        #endregion

        #region Methods

        private void UpdateInfo()
        {
            this.Info.SelectedLongTripForm = this.SelectedLongTripForm;

            this.ProtocolSystem.ChangeInfo(this.InfoPage, this.Info);
        }

        #endregion
    }
}