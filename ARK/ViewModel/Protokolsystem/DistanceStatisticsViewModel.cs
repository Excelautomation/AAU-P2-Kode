using System.Collections.Generic;
using ARK.Model;
using ARK.Model.DB;

namespace ARK.ViewModel.Protokolsystem
{
    internal class DistanceStatisticsViewModel : KeyboardContentViewModelBase
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