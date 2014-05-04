using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Model;
using ARK.ViewModel.Base;
using ARK.Model.DB;

namespace ARK.ViewModel.Administrationssystem
{
    public class FormsLongTripViewModel : ContentViewModelBase
    {

        private readonly DbArkContext _dbArkContext;
        private LongDistanceForm _LongDistanceForm;
        private bool _RecentChange = false;

        
        public FormsLongTripViewModel ()
        {
            _dbArkContext = DbArkContext.GetDbContext();



        }

        public bool RecentChange
        {
            get { return _RecentChange; }
            set { _RecentChange = value; Notify(); }
        }

        public LongDistanceForm LongDistanceForm { get { return _LongDistanceForm; } 
            set { 
                _LongDistanceForm = value; Notify(); 
            } }









    }
}
