using System;

namespace DifferentMethods.Univents
{
    public struct ScheduledInvoke : IComparable<ScheduledInvoke>
    {
        public float time;
        public ActionList univent;

        public int CompareTo(ScheduledInvoke other)
        {
            return this.time.CompareTo(other.time);
        }


    }

}