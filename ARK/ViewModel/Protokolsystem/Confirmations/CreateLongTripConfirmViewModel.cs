using System;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;
using System.Collections.ObjectModel;

namespace ARK.ViewModel.Protokolsystem.Confirmations
{
    public class CreateLongTripConfirmViewModel : ConfirmationViewModelBase
    {
        // Fields
        private LongTripForm _longTrip;
        private string _errors;

        public LongTripForm LongTrip 
        {
            get { return _longTrip; }
            set { 
                _longTrip = value; 
                Notify();
                UpdateErrors();
            }
        }

        public void UpdateErrors()
        {
            var sb = new StringBuilder();

            if (string.IsNullOrEmpty(LongTrip.TourDescription))
                sb.AppendLine(string.Format("Indtast venligst en turbeskrivelse"));

            if (string.IsNullOrEmpty(LongTrip.DistancesPerDay))
                sb.AppendLine(string.Format("Indtast venligst en distance"));

            if (string.IsNullOrEmpty(LongTrip.CampSites))
                sb.AppendLine(string.Format("Indtast venligst hvor I ønsker at overnatte"));

            if (LongTrip.PlannedStartDate <= DateTime.Now)
                sb.AppendLine(string.Format("Indtast venligst en gyldig startdato"));

            if (LongTrip.PlannedEndDate < DateTime.Now ||
                LongTrip.PlannedEndDate <= LongTrip.PlannedStartDate)
                sb.AppendLine(string.Format("Indtast venligst en gyldig slutdato"));

            if (!LongTrip.Members.Any())
                sb.AppendLine("Vælg venligst nogle medlemmer der skal med på langturen");

            if (LongTrip.ResponsibleMember == null)
                sb.AppendLine("Vælg venligst den ansvarlige person for langturen - vælg \"Vælg ansvarlig\" knappen til højre");

            Errors = sb.ToString();
        }

        public string Errors
        {
            get { return _errors; }
            set
            {
                _errors = value; 
                Notify();
            }
        }

        public ICommand SaveForm
        {
            get
            {
                return GetCommand(() =>
                {
                    LongTrip.Members = LongTrip.Members.Where(member => member.Id >= 0).ToList();

                    DbArkContext.GetDbContext().LongTripForm.Add(LongTrip);
                    DbArkContext.GetDbContext().SaveChanges();
                    
                    Hide();
                    ProtocolSystem.StatisticsDistance.Execute(null);
                }, () => LongTrip != null &&
                         !string.IsNullOrEmpty(LongTrip.TourDescription) &&
                         !string.IsNullOrEmpty(LongTrip.DistancesPerDay) &&
                         !string.IsNullOrEmpty(LongTrip.CampSites) &&
                         LongTrip.PlannedStartDate >= DateTime.Now &&
                         LongTrip.PlannedEndDate >= DateTime.Now &&
                         LongTrip.PlannedEndDate > LongTrip.PlannedStartDate &&
                         LongTrip.Members.Any() && 
                         LongTrip.ResponsibleMember != null);
            }
        }

        public ICommand CancelForm
        {
            get
            {
                return GetCommand(() =>
                {
                    Hide();
                    ProtocolSystem.StatisticsDistance.Execute(null);
                });
            }
        }

        public ICommand ChangeForm
        {
            get
            {
                return GetCommand(Hide);
            }
        }
    }
}