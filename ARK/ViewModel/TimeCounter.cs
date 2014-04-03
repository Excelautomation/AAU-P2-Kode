using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.ViewModel
{
    class TimeCounter
    {
        private static Stack<DateTime> stack = new Stack<DateTime>();
        public static void startTimer()
        {
            stack.Push(DateTime.Now);
        }

        public static TimeSpan stopTime()
        {
            return DateTime.Now - stack.Pop();
        }

        public static void stopTime(string name)
        {
            TimeSpan span = stopTime();

            Debug.WriteLine(name + " took " + span.TotalMilliseconds + " ms to execute");
        }
    }
}
