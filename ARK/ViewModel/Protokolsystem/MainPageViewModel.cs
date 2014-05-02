using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Model;
using System.Windows.Input;

namespace ARK.ViewModel.Protokolsystem
{
    class MainPageViewModel : Base.ViewModelBase
    {
        // constructor
        public MainPageViewModel()
        {
            // get members that rowed over 0km

            // sort members after km rowed this season

            // get boats out
        }

        // Props
        public List<Member> Members { get; set; }

        public List<Boat> BoatsOut { get; set; }

        public ICommand BoatSelected
        {
            get
            {
                return GetCommand<object>(x =>
                {
                    
                });
            }
        }

        public ICommand MemberSelected
        {
            get
            {
                return GetCommand<object>(x =>
                {

                });
            }
        }
    }
}
