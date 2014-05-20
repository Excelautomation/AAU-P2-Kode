using System;

namespace ARK.ViewModel.Protokolsystem
{
    public class KeyboardEventArgs : EventArgs
    {
        public KeyboardEventArgs(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }
    }
}