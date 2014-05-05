    using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using System.ComponentModel;
using ARK.Model.Extensions;
using ARK.View.Administrationssystem.Filters;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Administrationssystem
{
   public class TripsViewModel : ContentViewModelBase //, IFilterContentViewModel
    {
        private List<Trip> _TripsNonFiltered;
        private readonly DbArkContext _dbArkContext;
        private IEnumerable<Trip> _Trips;
        private Trip _currentTrip;
        //private FrameworkElement _filter;

       public TripsViewModel()
        {
            // Instaliser lister så lazy ikke fejler
            _TripsNonFiltered = new List<Trip>();
            
            // Load data
            _dbArkContext = DbArkContext.GetDbContext();
            Task.Factory.StartNew(() =>
            {
                DbArkContext db = DbArkContext.GetDbContext();

                lock (db)
                {
                    // Opret forbindelser Async
                    Task<List<Trip>> TripsLoad = db.Trip.ToListAsync();

                    _TripsNonFiltered = TripsLoad.Result;
                }

                Trips = _TripsNonFiltered;

                // Nulstil filter
                //ResetFilter();
            });

            // Nulstil filter
            //ResetFilter();

            // Setup filter
            //var filterController = new FilterContent(this);
            //filterController.EnableFilter(true, true);
            //filterController.FilterChanged += (o, eventArgs) => UpdateFilter(eventArgs); 
        }

       public IEnumerable<Trip> Trips
       {
           get { return _Trips; }
           set
           {
               _Trips = value;
               Notify();
           }
       }

       public Trip CurrentTrip
       {
           get { return _currentTrip; }
           set
           {
               _currentTrip = value;
               Notify();
           }
       }

       public ICommand SelectedChange
       {
           get
           {
               return GetCommand<Trip>(e =>
               {
                   CurrentTrip = e;
               });
           }
       }

       //public FrameworkElement Filter
       //{
       //    get { throw new NotImplementedException(); }
       //}
    }
}
