using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ARK.ViewModel
{
    public class TimeCounter
    {
        private static readonly Stack<DateTime> stack = new Stack<DateTime>();

        public static void StartTimer()
        {
            stack.Push(DateTime.Now);
        }

        public static TimeSpan StopTime()
        {
            return DateTime.Now - stack.Pop();
        }

        public static void StopTime(string name)
        {
            TimeSpan span = StopTime();

            Debug.WriteLine(name + " took " + span.TotalMilliseconds + " ms to execute");
        }
    }
}