using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Model;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Protokolsystem.Data
{
    class MemberDistanceViewModel : ViewModelBase
    {
        private Member _member;
        private double _distance;

        public MemberDistanceViewModel(Member member, double distance)
        {
            this.Member = member;
            this.Distance = distance;
        }

        public Member Member
        {
            get { return _member; }
            set { _member = value;
                Notify();
            }
        }

        public double Distance
        {
            get { return _distance; }
            set { _distance = value;
                Notify();
            }
        }
    }
}
