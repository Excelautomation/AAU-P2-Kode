using System;

namespace ARK.ViewModel.Protokolsystem
{
    public class KeyboardEventArgs : EventArgs
    {
        #region Constructors and Destructors

        public KeyboardEventArgs(string text)
        {
            this.Text = text;
        }

        #endregion

        #region Public Properties

        public string Text { get; private set; }

        #endregion
    }
}