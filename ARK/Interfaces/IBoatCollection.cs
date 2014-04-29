using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Model;

namespace ARK.Interfaces
{
    interface IBoatCollection<T>
    {
        Boat SelectedBoat { get; set; }

        IEnumerable<Boat> Boats { get; set; }

        void Sort(Func<Boat, T> predicate);
    }
}
