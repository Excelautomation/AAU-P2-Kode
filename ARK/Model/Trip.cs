using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARK.Model
{
    public class Trip : IEquatable<Trip>
    {
        public virtual Boat Boat { get; set; }

        public int BoatId { get; set; }

        public int CrewCount { get; set; }

        public string Direction { get; set; }

        public double Distance { get; set; }

        public int Id { get; set; }

        public bool LongTrip { get; set; }

        public virtual ICollection<Member> Members { get; set; }

        private ObservableCollection<Member> _observableMember;

        [NotMapped]
        public ObservableCollection<Member> ObservableMembers
        {
            get
            {
                if (_observableMember != null)
                    return _observableMember;

                _observableMember = new ObservableCollection<Member>(Members);
                _observableMember.CollectionChanged += (sender, e) =>
                {
                    switch (e.Action)
                    {
                        case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                            foreach (var item in e.NewItems)
                                Members.Add((Member)item);
                            break;
                        case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                            foreach (var item in e.OldItems)
                                Members.Remove((Member)item);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                };

                return _observableMember;
            }
        }

        public TimeSpan TimeBoatOut
        {
            get
            {
                if (TripEndedTime == null)
                {
                    return DateTime.Now - TripStartTime;
                }
                else
                {
                    return TripEndedTime.Value - TripStartTime;
                }
            }
        }

        public string Title { get; set; }

        public bool TripEnded
        {
            get
            {
                if (TripEndedTime != null && TripEndedTime != TripStartTime)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public DateTime? TripEndedTime { get; set; }

        public DateTime TripStartTime { get; set; }

        public bool Equals(Trip other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id == other.Id;
        }

        public static bool operator ==(Trip left, Trip right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Trip left, Trip right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Trip)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}