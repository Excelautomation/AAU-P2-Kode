using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Model
{
    public class Season
    {
        private DateTime _seaonStart;

        public DateTime SeasonStart
        {
            get { return _seaonStart; }
            set { _seaonStart = value; }
        }
    }
}
