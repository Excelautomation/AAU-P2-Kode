using System.Collections.Generic;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Protokolsystem
{
    internal class DistanceStatisticsViewModel : KeyboardContentViewModelBase
    {
        private List<Member> _members = new List<Member>();

        public DistanceStatisticsViewModel()
        {
            // Load data
            
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