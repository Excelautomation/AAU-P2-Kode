using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ARK.Model;
using ARK.Model.DB;

namespace ARK.HelperFunctions
{
    internal static class SeasonClass
    {
        private static Task _task;

        internal static void StartCheckCurrentSeasonEndTask(CancellationToken token)
        {
            if (_task == null)
            {
                _task = Task.Factory.StartNew(
                    async () =>
                    {
                        while (true)
                        {
                            CheckCurrentSeasonEnd();
                            await Task.Delay(new TimeSpan(3, 0, 0), token);
                        }
                    },
                    token);
            }
            else
            {
                throw new InvalidOperationException("The task has already been started");
            }
        }

        private static void CheckCurrentSeasonEnd()
        {
            using (var db = new DbArkContext())
            {
                Season currentSeason;

                // Get current season
                if (!db.Season.Any(x => true))
                {
                    currentSeason = new Season();
                    db.Season.Add(currentSeason);
                }
                else
                {
                    currentSeason = db.Season.AsEnumerable().Last(x => true);
                }

                // if current seasonEnd is before today add new season.
                if (DateTime.Compare(currentSeason.SeasonEnd, DateTime.Now) <= 0)
                {
                    currentSeason = new Season();
                    db.Season.Add(currentSeason);
                    db.SaveChanges();
                }
            }
        }
    }
}
