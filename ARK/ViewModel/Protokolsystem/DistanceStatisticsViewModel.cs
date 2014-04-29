using System;
using System.Collections.Generic;
using System.Linq;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Interfaces;
using System.Text;
using System.Threading.Tasks;

namespace ARK.ViewModel.Protokolsystem
{
    class DistanceStatisticsViewModel : KeyboardContentViewModelBase
    {
        private List<Member> _members = new List<Member>();

        public DistanceStatisticsViewModel() 
        {
            // Load data
            using (DbArkContext db = new DbArkContext())
            {

            }
        }
        
        public List<Member> Members
        {
            get { return _members; }
            set
            {
                _members = value;
                Notify();
            }
        }
    }
}
