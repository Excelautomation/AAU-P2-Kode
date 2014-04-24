using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ARK.ViewModel
{
    public static class TimeCounter
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
            StopTime(name, m => Debug.WriteLine(m));
        }

        public static void StopTime(string name, Action<string> outputMethod)
        {
            TimeSpan span = StopTime();

            outputMethod(name + " took " + span.TotalMilliseconds + " ms to execute");
        }
    }
}