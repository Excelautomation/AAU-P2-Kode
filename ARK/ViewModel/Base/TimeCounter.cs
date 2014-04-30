using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ARK.ViewModel.Base
{
    public static class TimeCounter
    {
        private static readonly Stack<DateTime> Stack = new Stack<DateTime>();

        public static void StartTimer()
        {
            Stack.Push(DateTime.Now);
        }

        public static TimeSpan StopTimeCount()
        {
            return DateTime.Now - Stack.Pop();
        }

        public static void StopTime([CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            StopTime("File: " + sourceFilePath + ":" + sourceLineNumber + "\n" + memberName
            , m => Debug.WriteLine(m));
        }

        public static void StopTime(string name, Action<string> outputMethod)
        {
            TimeSpan span = StopTimeCount();

            outputMethod(name + " took " + span.TotalMilliseconds + " ms to execute");
        }
    }
}