using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;

namespace ARK.ViewModel.Protokolsystem.Confirmations
{
    public class CreateLongTripConfirmViewModel : ConfirmationViewModelBase
    {
        // Fields
        #region Fields

        private string _errors;

        private LongTripForm _longTrip;

        #endregion

        #region Public Properties

        public ICommand CancelForm
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            this.Hide();
                            this.ProtocolSystem.StatisticsDistance.Execute(null);
                        });
            }
        }

        public ICommand ChangeForm
        {
            get
            {
                return this.GetCommand(this.Hide);
            }
        }

        public string Errors
        {
            get
            {
                return this._errors;
            }

            set
            {
                this._errors = value;
                this.Notify();
            }
        }

        public LongTripForm LongTrip
        {
            get
            {
                return this._longTrip;
            }

            set
            {
                this._longTrip = value;
                this.Notify();
                this.UpdateErrors();
            }
        }

        public ICommand SaveForm
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            this.LongTrip.Members = this.LongTrip.Members.Where(member => member.Id >= 0).ToList();

                            DbArkContext.GetDbContext().LongTripForm.Add(this.LongTrip);
                            DbArkContext.GetDbContext().SaveChanges();

                            this.Hide();
                            this.ProtocolSystem.StatisticsDistance.Execute(null);
                        }, 
                    () =>
                    this.LongTrip != null && !string.IsNullOrEmpty(this.LongTrip.TourDescription)
                    && !string.IsNullOrEmpty(this.LongTrip.DistancesPerDay)
                    && !string.IsNullOrEmpty(this.LongTrip.CampSites) && this.LongTrip.PlannedStartDate >= DateTime.Now
                    && this.LongTrip.PlannedEndDate >= DateTime.Now
                    && this.LongTrip.PlannedEndDate > this.LongTrip.PlannedStartDate && this.LongTrip.Members.Any()
                    && this.LongTrip.ResponsibleMember != null);
            }
        }

        #endregion

        #region Public Methods and Operators

        public void UpdateErrors()
        {
            var sb = new StringBuilder();

            if (string.IsNullOrEmpty(this.LongTrip.TourDescription))
            {
                sb.AppendLine(string.Format("Indtast venligst en turbeskrivelse"));
            }

            if (string.IsNullOrEmpty(this.LongTrip.DistancesPerDay))
            {
                sb.AppendLine(string.Format("Indtast venligst en distance"));
            }

            if (string.IsNullOrEmpty(this.LongTrip.CampSites))
            {
                sb.AppendLine(string.Format("Indtast venligst hvor I ønsker at overnatte"));
            }

            if (this.LongTrip.PlannedStartDate <= DateTime.Now)
            {
                sb.AppendLine(string.Format("Indtast venligst en gyldig startdato"));
            }

            if (this.LongTrip.PlannedEndDate < DateTime.Now
                || this.LongTrip.PlannedEndDate <= this.LongTrip.PlannedStartDate)
            {
                sb.AppendLine(string.Format("Indtast venligst en gyldig slutdato"));
            }

            if (!this.LongTrip.Members.Any())
            {
                sb.AppendLine("Vælg venligst nogle medlemmer der skal med på langturen");
            }

            if (this.LongTrip.ResponsibleMember == null)
            {
                sb.AppendLine(
                    "Vælg venligst den ansvarlige person for langturen - vælg \"Vælg ansvarlig\" knappen til højre");
            }

            this.Errors = sb.ToString();
        }

        #endregion
    }
}