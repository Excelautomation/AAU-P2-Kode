using System;

namespace ARK.Model
{
    public class Season
    {
        public Season()
        {
            SeasonStart = DateTime.Now;
            SeasonEnd = SeasonStart;
            SeasonEnd = SeasonEnd.AddYears(1);
        }

        public DateTime EarliestSeasonEnd
        {
            get
            {
                return SeasonStart.AddDays(183);
            }
        }

        public int Id { get; set; }

        public DateTime LatestSeasonEnd
        {
            get
            {
                return SeasonStart.AddDays(365 + 183);
            }
        }

        public DateTime SeasonEnd { get; set; }

        public DateTime SeasonStart { get; set; }
    }
}