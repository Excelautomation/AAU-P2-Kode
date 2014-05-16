﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ARK.Model;
using ARK.Model.DB;
using ARK.Model.XML;

namespace ARK.HelperFunctions.SMSGateway
{
    public class DemoSMSGateway : ISmsGateway
    {
        #region Public Methods and Operators

        public bool SendSms(string sender, string reciever, string message)
        {
            Debug.WriteLine("Sender sms til " + reciever + " fra " + sender + " med beskeden " + message);
            return true;
        }

        #endregion
    }

    public class SmsWarnings
    {
        #region Constants

        private const string MessageInvalidResponse = "Bekræftelsen blev ikke modtaget";

        private const string MessageNotHome =
            "Hej {0} Bekræft venligst med OK, at du har det godt. Venlig hilsen Aalborg Roklub";

        private const string MessageNotHomeAdministration = "Nogle af medlemmerne på rotur er ikke kommet hjem endnu";

        private const string MessageValidResponse = "Bekræftelsen blev succesfuldt modtaget";

        private const string Sender = "ARK";

        #endregion

        #region Constructors and Destructors

        public SmsWarnings()
        {
            this.Gateway = new SmsIt("0000000000000000");
        }

        #endregion

        #region Public Properties

        public ISmsGateway Gateway { get; set; }

        #endregion

        #region Public Methods and Operators

        public void Test()
        {
            // Gør kun dette her når solen er nede
            if (this.IsAfterSunset() || true)
            {
                using (var db = new DbArkContext())
                {
                    var warnings = this.GetTripWarningSms(db).ToList();
                    var responses = db.GetSMS.ToList();

                    this.HandleWarningSms(warnings);
                    this.HandleResponseSms(warnings, responses);
                    this.HandleNoResponseSms(warnings, db);

                    // Fjern tidligere sms'er
                    db.GetSMS.RemoveRange(responses);

                    db.SaveChanges();
                }
            }
        }

        #endregion

        #region Methods

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
            // Find all trips which dosn't have a tripwarningsms yet - select tripWarningSms and add to db
            IEnumerable<TripWarningSms> missingTrips =
                (from trip in db.Trip where trip.TripEndedTime == null && !trip.LongTrip select trip).ToList()
                    .Where(trip => !db.TripWarningSms.Any(warning => warning.Trip.Id == trip.Id))
                    .Select(trip => new TripWarningSms { Trip = trip })
                    .ToList();

            // Remove trips home
            IEnumerable<TripWarningSms> endedTrips =
                db.TripWarningSms.Where(warning => warning.Trip.TripEndedTime != null).ToList();

            // Update database
            if (missingTrips.Any())
            {
                db.TripWarningSms.AddRange(missingTrips);
            }

            if (endedTrips.Any())
            {
                db.TripWarningSms.RemoveRange(endedTrips);
            }

            return
                db.TripWarningSms.Where(warning => warning.RecievedSms == null)
                    .Where(warning => warning.Trip.TripEndedTime == null)
                    .Include(warning => warning.Trip);
        }

        private void HandleNoResponseSms(IEnumerable<TripWarningSms> warnings, DbArkContext db)
        {
            foreach (
                var warn in
                    warnings.Where(warn => !warn.RecievedSms.HasValue)
                        .Where(warn => warn.SentSms.HasValue && !warn.SentAdminSms.HasValue)
                        .AsEnumerable()
                        .Where(warn => (DateTime.Now - warn.SentSms.Value).TotalMinutes > 15))
            {
                var numbers = db.Admin.Where(a => a.ContactDark && a.Member.Phone != null).Select(a => a.Member.Phone);

                foreach (var number in numbers)
                {
                    this.Gateway.SendSms(Sender, number, MessageNotHomeAdministration);
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
                        warning => warning.Trip.Members.Any(member => member.Phone == response.From.Replace("+45", string.Empty)))
                        .ToList();

                if (warn.Any())
                {
                    if (response.Text.ToLower() == "ok")
                    {
                        warn[0].RecievedSms = response.RecievedDate;
                        this.Gateway.SendSms(Sender, response.From.Replace("+45", string.Empty), MessageValidResponse);
                    }
                    else
                    {
                        this.Gateway.SendSms(Sender, response.From.Replace("+45", string.Empty), MessageInvalidResponse);
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
                    this.Gateway.SendSms(
                        Sender, 
                        member.Phone, 
                        string.Format(MessageNotHome, member.FirstName + " " + member.LastName));
                }

                sms.SentSms = DateTime.Now;
            }
        }

        private bool IsAfterSunset()
        {
            return XmlParser.GetSunsetFromXml() > DateTime.Now;
        }

        #endregion
    }
}