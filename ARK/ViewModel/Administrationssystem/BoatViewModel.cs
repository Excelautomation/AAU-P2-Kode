using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Extensions;
using ARK.View.Administrationssystem.Filters;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Administrationssystem
{
    public class BoatViewModel : ContentViewModelBase, IFilterContentViewModel
    {
        #region Fields

        private IEnumerable<Boat> _boats;

        private List<Boat> _boatsNonFiltered;

        private Boat _currentBoat;

        private FrameworkElement _filter;

        private Member _mostUsingMember;

        private bool _recentCancel;

        private bool _recentSave;

        private IEnumerable<Trip> _trips;

        #endregion

        #region Constructors and Destructors

        public BoatViewModel()
        {
            this.ParentAttached += (sender, e) =>
                {
                    // Load data
                    using (var db = new DbArkContext())
                    {
                        this._boatsNonFiltered =
                            db.Boat.Include(boat => boat.DamageForms).Include(boat => boat.Trips).ToList();

                        this._trips = db.Trip.Include(trip => trip.Members).Include(trip => trip.Boat).ToList();
                    }

                    // Nulstil filter
                    this.ResetFilter();

                    // Sæt valgt båd
                    if (this.Boats.Count() != 0)
                    {
                        this.CurrentBoat = this.Boats.First();
                    }
                };

            // Setup filter
            var filterController = new FilterContent(this);
            filterController.EnableFilter(true, true);
            filterController.FilterChanged += (o, eventArgs) => this.UpdateFilter(eventArgs);
        }

        #endregion

        #region Public Properties

        public IEnumerable<Boat> Boats
        {
            get
            {
                return this._boats;
            }

            set
            {
                this._boats = value;
                this.Notify();
            }
        }

        public ICommand CancelChanges
        {
            get
            {
                return this.GetCommand(this.Reload);
            }
        }

        public Boat CurrentBoat
        {
            get
            {
                return this._currentBoat;
            }

            set
            {
                if (this._currentBoat != null)
                {
                    this.Reload();
                }

                this._currentBoat = value;

                if (this._currentBoat != null)
                {
                    this.MostUsingMember = this.MostUsingMemberFunc();
                }
                else
                {
                    this.MostUsingMember = null;
                }

                this.RecentSave = false;
                this.RecentCancel = false;
                this.Notify();
            }
        }

        public FrameworkElement Filter
        {
            get
            {
                return this._filter ?? (this._filter = new BoatsFilter());
            }
        }

        public Member MostUsingMember
        {
            get
            {
                return this._mostUsingMember;
            }

            set
            {
                this._mostUsingMember = value;
                this.Notify();
            }
        }

        public bool RecentCancel
        {
            get
            {
                return this._recentCancel;
            }

            set
            {
                if (value != this._recentCancel)
                {
                    this._recentCancel = value;
                    this._recentSave = false;
                    this.NotifyCustom("RecentSave");
                }

                this.Notify();
            }
        }

        public bool RecentSave
        {
            get
            {
                return this._recentSave;
            }

            set
            {
                if (value != this._recentSave)
                {
                    this._recentSave = value;
                    this._recentCancel = false;
                    this.NotifyCustom("RecentCancel");
                }

                this.Notify();
            }
        }

        public ICommand SaveChanges
        {
            get
            {
                return this.GetCommand(this.Save);
            }
        }

        #endregion

        #region Public Methods and Operators

        public Member MostUsingMemberFunc()
        {
            if (this.CurrentBoat.Trips.Count == 0)
            {
                return null;
            }

            IEnumerable<Member> members =
                this._trips.Where(trip => trip.Boat.Id == this.CurrentBoat.Id).SelectMany(trip => trip.Members);

            return
                (from member in members.Distinct()
                 orderby members.Count(m => m.Id == member.Id) descending
                 select member).First();
        }

        public void OpenBoat(Boat boat)
        {
            this.CurrentBoat = this.Boats.First(b => b.Id == boat.Id);
        }

        #endregion

        #region Methods

        private void Reload()
        {
            using (var db = new DbArkContext())
            {
                db.Entry(this.CurrentBoat).State = EntityState.Modified;
                db.Entry(this.CurrentBoat).Reload();
            }

            this.RecentCancel = true;

            // Trigger notify - reset lists
            IEnumerable<Boat> tmp = this.Boats;
            this.Boats = null;
            this.Boats = tmp;

            this.NotifyCustom("CurrentBoat");
        }

        private void ResetFilter()

        {
            this.Boats = this._boatsNonFiltered.AsReadOnly();
        }

        private void Save()
        {
            using (var db = new DbArkContext())
            {
                db.Entry(this.CurrentBoat).State = EntityState.Modified;
                db.SaveChanges();
            }

            this.RecentSave = true;
        }

        private void UpdateFilter(FilterChangedEventArgs args)
        {
            // Nulstil filter
            this.ResetFilter();

            // Tjek om en af filtertyperne er aktive
            if ((args.FilterEventArgs == null || !args.FilterEventArgs.Filters.Any())
                && (args.SearchEventArgs == null || string.IsNullOrEmpty(args.SearchEventArgs.SearchText)))
            {
                return;
            }

            // Tjek filter
            if (args.FilterEventArgs != null && args.FilterEventArgs.Filters.Any())
            {
                this.Boats = FilterContent.FilterItems(this.Boats, args.FilterEventArgs);
            }

            // Tjek søgning
            if (args.SearchEventArgs != null && !string.IsNullOrEmpty(args.SearchEventArgs.SearchText))
            {
                this.Boats = from boat in this.Boats where boat.Filter(args.SearchEventArgs.SearchText) select boat;
            }
        }

        #endregion
    }
}