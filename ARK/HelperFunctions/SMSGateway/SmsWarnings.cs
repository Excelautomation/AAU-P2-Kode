using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ARK.Model;
using ARK.Model.DB;
using ARK.Model.XML;

namespace ARK.HelperFunctions.SMSGateway
{
    public class DemoSMSGateway : ISmsGateway
    {
        public bool SendSms(string sender, string reciever, string message)
        {
            Debug.WriteLine("Sender sms til " + reciever + " fra " + sender + " med beskeden " + message);
            return true;
        }
    }

    public class SmsWarnings
    {
        private const string MessageInvalidResponse = "Bekræftelsen blev ikke modtaget";

        private const string MessageNotHome =
            "Hej {0} Bekræft venligst med OK, at du har det godt. Venlig hilsen Aalborg Roklub";

        private const string MessageNotHomeAdministration = "Nogle af medlemmerne på rotur er ikke kommet hjem endnu";

        private const string MessageValidResponse = "Bekræftelsen blev succesfuldt modtaget";

        private const string Sender = "ARK";

        private int _smsDelay;

        private int _smsWait;

        private static Task _task;

        // Methods
        internal static void RunTask(CancellationToken token)
        {
            if (_task == null)
            {
                _task = Task.Factory.StartNew(
                    async () =>
                        {
                            var warn = new SmsWarnings();
                            while (true)
                            {
                                warn.UpdateSmsSettings();
                                warn.DoWork();
                                await Task.Delay(new TimeSpan(0, 0, 5), token);
                            }
                        }, 
                    token);
            }
            else
            {
                throw new InvalidOperationException("The task has already been started");
            }
        }

        public SmsWarnings()
        {
            Gateway = new DemoSMSGateway();
        }

        public ISmsGateway Gateway { get; set; }

        private void DoWork()
        {
            // Do this after sunest plus optional extra time
            if (IsAfterSunsetAndWait())
            {
                using (var db = new DbArkContext())
                {
                    var warnings = GetTripWarningSms(db).ToList();
                    var responses = db.GetSMS.ToList();

                    HandleWarningSms(warnings);
                    HandleResponseSms(warnings, responses);
                    HandleNoResponseSms(warnings, db);

                    // Fjern tidligere sms'er
                    db.GetSMS.RemoveRange(responses);

                    db.SaveChanges();
                }
            }
        }

        private IEnumerable<Trip> GetCurrentTrips()
        {
            IEnumerable<Trip> output;

            using (var db = new DbArkContext())
            {
                output = db.Trip.Where(trip => trip.TripEndedTime == null && !trip.LongTrip).ToList();
            }

            return output;
        }

        private IEnumerable<TripWarningSms> GetTripWarningSms(DbArkContext db)
        {
            // Find all trips which dosn't have a tripwarningsms yet - create a tripWarningSms and add to db
            IEnumerable<TripWarningSms> missingTrips =
                (from trip in db.Trip where trip.TripEndedTime == null && !trip.LongTrip select trip).ToList()
                    .Where(trip => !db.TripWarningSms.Any(warning => warning.Trip.Id == trip.Id))
                    .Where(trip => trip.Boat.SpecificBoatType != Boat.BoatType.Ergometer)
                    .Select(trip => new TripWarningSms { Trip = trip })
                    .ToList();

            // Getting all trips that has ended
            IEnumerable<TripWarningSms> endedTrips =
                db.TripWarningSms.Where(warning => warning.Trip.TripEndedTime != null).ToList();

            // Update database
            if (missingTrips.Any())
            {
                db.TripWarningSms.AddRange(missingTrips);   // adds trips that is not yet finished
            }

            if (endedTrips.Any())
            {
                db.TripWarningSms.RemoveRange(endedTrips);  // removes ended trips
            }

            return
                db.TripWarningSms.Where(warning => warning.RecievedSms == null)
                    .Where(warning => warning.Trip.TripEndedTime == null)
                    .Include(warning => warning.Trip);
        }

        private void HandleNoResponseSms(IEnumerable<TripWarningSms> warnings, DbArkContext db)
        {
            foreach (var warn in
                warnings.Where(warn => !warn.RecievedSms.HasValue)
                    .Where(warn => warn.SentSms.HasValue && !warn.SentAdminSms.HasValue)
                    .AsEnumerable()
                    .Where(warn => (DateTime.Now - warn.SentSms.Value).TotalMinutes > _smsDelay))
            {
                var numbers = db.Admin.Where(a => a.ContactDark && a.Member.Phone != null).Select(a => a.Member.Phone);

                foreach (var number in numbers)
                {
                    Gateway.SendSms(Sender, number, MessageNotHomeAdministration);
                }

                warn.SentAdminSms = DateTime.Now;
            }
        }

        private void HandleResponseSms(IEnumerable<TripWarningSms> warnings, IEnumerable<GetSMS> responses)
        {
            foreach (var response in responses)
            {
                var warn =
                    warnings.Where(
                        warning =>
                        warning.Trip.Members.Any(member => member.Phone == response.From.Replace("+45", string.Empty)))
                        .ToList();

                if (warn.Any())
                {
                    if (response.Text.ToLower() == "ok")
                    {
                        warn[0].RecievedSms = response.RecievedDate;
                        Gateway.SendSms(Sender, response.From.Replace("+45", string.Empty), MessageValidResponse);
                    }
                    else
                    {
                        Gateway.SendSms(Sender, response.From.Replace("+45", string.Empty), MessageInvalidResponse);
                    }
                }
            }
        }

        private void HandleWarningSms(IEnumerable<TripWarningSms> warnings)
        {
            var pending = warnings.Where(warning => warning.SentSms == null);

            foreach (var sms in pending)
            {
                foreach (var member in sms.Trip.Members.Where(member => !string.IsNullOrEmpty(member.Phone)))
                {
                    Gateway.SendSms(
                        Sender, 
                        member.Phone, 
                        string.Format(MessageNotHome, member.FirstName + " " + member.LastName));
                }

                sms.SentSms = DateTime.Now;
            }
        }

        private bool IsAfterSunsetAndWait()
        {
            return DateTime.Now > SunsetClass.Sunset.AddMinutes(_smsWait);
        }

        private void UpdateSmsSettings()
        {
            using (var db = new DbArkContext())
            {
                Setting temp;
                _smsDelay = (temp = db.Settings.FirstOrDefault(x => x.Name == "SmsDelay")) != null
                                ? Convert.ToInt32(temp.Value)
                                : 15;
                _smsWait = (temp = db.Settings.FirstOrDefault(x => x.Name == "SmsWait")) != null
                               ? Convert.ToInt32(temp.Value)
                               : 5;
            }
        }
    }
}