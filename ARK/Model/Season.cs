using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Model
{
    public class Season
    {
        #region Constructors and Destructors

        public Season()
        {
            this.SeasonStart = DateTime.Now;
            this.SeasonEnd = this.SeasonStart;
            this.SeasonEnd = this.SeasonEnd.AddYears(1);
        }

        #endregion

        #region Public Properties

        public DateTime EarliestSeasonEnd
        {
            get
            {
                return this.SeasonStart.AddDays(183);
            }
        }

        public int Id { get; set; }

        public DateTime LatestSeasonEnd
        {
            get
            {
                return this.SeasonStart.AddDays(365 + 183);
            }
        }

        public DateTime SeasonEnd { get; set; }

        public DateTime SeasonStart { get; set; }

        #endregion
    }
}